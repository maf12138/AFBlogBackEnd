using Infrasturacture;
using Domain;
using Domain.Extensions.JWT;
using Swashbuckle.AspNetCore.SwaggerGen;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;
using BackEndWebAPI.Mappers;
using Microsoft.AspNetCore.Mvc;
using BackEndWebAPI.Filters;
using BackEndWebAPI.Configs;
using MySqlConnector;
using Domain.Interface;
using Infrasturacture.Dapper;
using BackEndWebAPI.Middleware;
using FluentValidation.AspNetCore;
using FluentValidation;
using BackEndWebAPI.DTO;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
# region 第三方库配置
//注册MediatR,参数一般指定事件处理者所在的若干个程序集，参考GitHubwiki ：https://github.com/jbogard/MediatR/wiki
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
//注册AutoMapper
builder.Services.AddAutoMapper(typeof(ArticleMapProfile).Assembly, typeof(UserMapperProfile).Assembly, typeof(TagMapProfile).Assembly);
//注册EF Core的DbContext
builder.Services.AddDbContext<BlogDbContext>(opt =>
{
    string connStr = Environment.GetEnvironmentVariable("MAF_MYSQL_CONN");//builder.Configuration.GetSection("MafMySQLConn").Value;//.GetConnectionString("MySQLConn");//也可用该方法-使用appsetting.json中存储的连接字符串
    opt.UseMySql(connStr, new MySqlServerVersion(new Version(8, 0, 32)));
}, ServiceLifetime.Scoped);//向容器注入BlogDbContext
//使用 第三方库Zack.AnyDBConfigProvider,从数据库T_Configs表中读取部分配置,数据库连接字符串从用户机密中读取
var webBuilder = builder.WebHost;
webBuilder.ConfigureAppConfiguration((hostCtx, configBuilder) => {
    string connStr = Environment.GetEnvironmentVariable("MAF_MYSQL_CONN"); // builder.Configuration.GetSection("MafMySQLConn").Value;//.NET 6写法,之前的写法可以看文档
    configBuilder.AddDbConfiguration(() => new MySqlConnection(connStr), reloadOnChange: true, reloadInterval: TimeSpan.FromSeconds(2));// 这里AddDbConfiguration以扩展方法的形式配置了连接上对应的表
});
//注册FluentValidation
https://github.com/FluentValidation/FluentValidation/blob/main/docs/aspnet.md
//推荐手动注册,写多了就使用扩展方法注册
//builder.Services.AddValidatorsFromAssemblyContaining<UserDTOValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<Program>();
//builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddScoped<IValidator<UserDTO>, UserDTOValidator>();
# endregion
# region jwt配置
//注册jwt配置实例,可以改为从数据库读取
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWTOptions"));//配置
JWTOptions jwtOpt = builder.Configuration.GetSection("JWTOptions").Get<JWTOptions>();//用get进行绑定
builder.Services.AddJWTAuthentication(jwtOpt);
# endregion
#region 注册自定义服务
//注入获取Mysql连接的工厂类
builder.Services.AddScoped<IDbConnectionFactory, DapperDbConnectionFactory>();
//注册自定义的Token Service
builder.Services.AddScoped<ITokenService, TokenService>();
//注册认证相关仓储,领域服务等
builder.Services.AddScoped<IdentityDomainService>();
builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
//注册文章相关服务
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ArticleDomainService>();
//注册评论相关服务
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<CommentDomainService>();
# endregion
# region //绑定配置
//跨域配置
builder.Services.Configure<CORSConfig>(builder.Configuration.GetSection("CORSORIGN"));
var origins = builder.Configuration.GetSection("CORSORIGN").Get<CORSConfig>();
//有需要可以在controller上加上[EnableCors("AllowAll")],使用特性也可以解决跨域
//string[] urls = new []{ "http://localhost:8080","http://localhost:3000"};
var urls = origins.urls;//允许跨域访问的地址
Console.WriteLine(urls.ToString());
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(builder => builder.WithOrigins(urls)
    .AllowAnyMethod().AllowAnyHeader().AllowCredentials()));
//从配置数据库中读取七牛云配置--见第三方库Zack.AnyDBConfigProvider
builder.Services.Configure<QiNiuOptions>(builder.Configuration.GetSection("QiNiuKey"));
#endregion
# region 配置内置框架如MVC,Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication();
//配置swagger小锁头
builder.Services.Configure<SwaggerGenOptions>(c =>
{
    c.AddAuthenticationHeader();
});
//内存缓存
builder.Services.AddMemoryCache();
//控制器
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;//配置循环不报错,似乎配置automapper深度才是推荐的,或者使用poco类
});
//MVC配置
builder.Services.Configure<MvcOptions>(opt =>
{
    opt.Filters.Add<JWTValidationFilter>();//配置JWT筛选器用于检查JWT的Version
    opt.Filters.Add<GlobalExceptionFilter>();//配置全局异常筛选器,会覆盖掉默认的异常处理,中间件等,后台线程的异常不会被捕获
    opt.Filters.Add<TransactionScopeFilter>();//配置自动启用事务的操作筛选器
    opt.Filters.Add<RateLimitFilter>();//配置限流操作筛选器
});
# endregion
# region 注册并配置Identity
builder.Services.AddDataProtection();
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;//密码是否需要数字
    options.Password.RequireLowercase = false;//密码是否需要小写字母
    options.Password.RequireUppercase = false;//密码是否需要大写字母
    options.Password.RequireNonAlphanumeric = false;//密码是否需要特殊符号如@#等
    options.Password.RequiredLength = 6;//密码长度
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;//重置密码令牌属性=默认邮箱提供者
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;//邮箱确认令牌提供者属性=默认邮箱提供者
});
var idBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);
idBuilder.AddEntityFrameworkStores<BlogDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultTokenProviders()
    .AddUserManager<UserManager<User>>()
    .AddRoleManager<RoleManager<Role>>();
#endregion
# region 注册日志
//注册控制台日志
builder.Services.AddLogging(logBuilder => { logBuilder.AddConsole(); });
# endregion
# region 注册自定义中间件
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
# endregion
var app = builder.Build();
// Configure the HTTP request pipeline.配置HTTP请求管道
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
# region 配置静态文件中间件
var docpath = Path.Combine(Directory.GetCurrentDirectory(), "docs");//在Linux中使用绝对路径,这里不可用,或者需要手动创建一下?
if (!Directory.Exists(docpath))
{
    // 创建目录
    Directory.CreateDirectory(docpath);
    Console.WriteLine("目录已创建！");
}
var fileProvider = new PhysicalFileProvider(docpath);//服务器文件物理路径
var videoPath = Path.Combine(Directory.GetCurrentDirectory(), "medias");
if (!Directory.Exists(videoPath))
{
    // 创建目录
    Directory.CreateDirectory(videoPath);
    Console.WriteLine("目录已创建！");
}
var videoFileProvider = new PhysicalFileProvider(videoPath);
if (app.Environment.IsProduction())
{
    var docDefaultFileOptions = new DefaultFilesOptions()
    {
        FileProvider = fileProvider,
        RequestPath = "/blog"//请求路径
    };
    var videoDefaultFileOptions = new DefaultFilesOptions()
    {
        FileProvider = videoFileProvider,
        RequestPath = "/media"
    };
    docDefaultFileOptions.DefaultFileNames.Add("Readme.html");
    app.UseDefaultFiles(docDefaultFileOptions).UseDefaultFiles().UseDefaultFiles(videoDefaultFileOptions);//链式
}

//注册wwwroot静态文件服务器与目录浏览中间件
app.UseStaticFiles().UseDirectoryBrowser();

// 配置 docs 与 medias 文件夹
var docStaticFileOptions = new StaticFileOptions()
{
    FileProvider = fileProvider,
    RequestPath = "/blog"     
};

var docDirectoryBrowserOptions = new DirectoryBrowserOptions
{
    FileProvider = fileProvider,
    RequestPath = "/blog"//请求路径
};

var videoStaticFileOptions = new StaticFileOptions()
{
    FileProvider = videoFileProvider,
    RequestPath = "/media"
};
var videoDirectoryBrowserOptions = new DirectoryBrowserOptions()
{
    FileProvider = videoFileProvider,
    RequestPath = "/media"
};

app.UseStaticFiles(videoStaticFileOptions).UseDirectoryBrowser(videoDirectoryBrowserOptions);
app.UseStaticFiles(docStaticFileOptions).UseDirectoryBrowser(docDirectoryBrowserOptions); //注意中间件use(也就是注册)顺序,官方有介绍静态文件的授权
# endregion
//app.UseHttpsRedirection();//需要在服务器上配置证书,关掉可以优化性能,证书配置在nginx上
app.UseForwardedHeaders();//这个可以获取到客户端IP地址
app.UseCors();// 跨域
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>(); //使用全局异常处理中间件,不推荐使用
app.MapControllers();
app.Run();
