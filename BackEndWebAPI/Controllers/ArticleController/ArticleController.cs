using AutoMapper;
using BackEndWebAPI.WebAPIExtensions;
using Domain;
using Domain.Entities;
using Infrasturacture;
using BackEndWebAPI.VO;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using BackEndWebAPI.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BackEndWebAPI.Controllers.ArticleController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        //对于大部分应用场景来讲，一次对控制器中方法的调用就对应一个工作单元，因此我们可以开发一个在控制器的方法调用结束后自动调用SaveChangesAsync的机制[UnitOfWorkAttribute]。这样就能大大简化应用服务层代码的编写，从而避免对SaveChangesAsync方法的重复调用。当然，对于特殊的应用服务层代码，我们可能仍然需要手动决定调用SaveChangesAsync方法的时机
        private readonly IArticleRepository _articleRepository;
        private readonly BlogDbContext _blogDbContext;//用来手动保存更改的
        private readonly IIdentityRepository _identityRepository;

        private readonly string _config;

        private readonly IMapper _mapper;

        private readonly IdentityDomainService _identityDomainService;
        public ArticleController(IArticleRepository articleRepository, IIdentityRepository identityRepository, BlogDbContext blogDbContext
           , IMapper mapper,IdentityDomainService identityDomainService )
        {
            _articleRepository = articleRepository;
            _identityRepository = identityRepository;            
            _config = Directory.GetCurrentDirectory() + "\\docs";
            _blogDbContext = blogDbContext;
            _mapper = mapper;
            _identityDomainService = identityDomainService;
        }

        [HttpGet("/api/article/hotArticleList")]
        public async Task<ResponseResult<IEnumerable<HotArticleVo>>> GetHotArticles()
        {
            //如何优化?
            var Articles = (await _articleRepository.GetAllArticlesAndDetailAsync()).OrderByDescending(article => article.ViewCount).Take(10);
            var hotArticles = _mapper.Map<IEnumerable<HotArticleVo>>(Articles);
            return ResponseResult<IEnumerable<HotArticleVo>>.OkResult(hotArticles);
        }

        // GET: api/<ArticleController>
        [HttpGet("/api/GetAllArticlesAsync")]
        //[Authorize(Roles ="Admin")]
        public Task<IEnumerable<Article>> Get()
        {
            return _articleRepository.GetAllArticlesAsync();
        }
        //获取文章,分类,标签的数量
        [HttpGet("/api/article/count")]
        public async Task<ResponseResult<WebSiteCount>> GetArticleCount()
        {
            var article= await _blogDbContext.Articles.CountAsync();
            var category = await _blogDbContext.Categories.CountAsync();
            var tag = await _blogDbContext.Tags.CountAsync();   
            return ResponseResult<WebSiteCount>.OkResult(new WebSiteCount(article,category,tag));
        }
        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/article/{id}")]
        //ActionResult学习 https://www.cnblogs.com/kklldog/p/aspnetcore-actionresult.html
        public async Task<ResponseResult<ArticleDetailsVo>> GetArticleDetail(int id)
        {
            var findResult = await _articleRepository.FindArticleByIdAsync(id);
            if (findResult == null)
            {
                return ResponseResult<ArticleDetailsVo>.ErrorResult(404,"未找到该文章");
            }
            var findResult1 =await _articleRepository.FindArticleByIdIncludeAsync(id);//包含导航属性,这居然能影响最终结果!!!!,这个变量明明没有使用.
            var articleDto = _mapper.Map<ArticleDetailsVo>(findResult);//map的时候会自动加载导航属性,不需要再次加载,配置好规则就行
            return new ResponseResult<ArticleDetailsVo>(200, "操作成功", articleDto);
        }
        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="pageNum">当前页码 必须</param>
        /// <param name="pageSize">每页条数 必须</param>
        /// <param name="categoryId">分类Id可选</param>
        /// <param name="tagId">标签id可选</param>
        /// <param name="date">年份/月份 可选</param>
        /// <returns></returns>
        [HttpGet("/api/article/articleList")]
        public async Task<ResponseResult<PageVo<ArticleListVo>>> GetArticleList([FromQuery] int? pageNum, [FromQuery] int pageSize, [FromQuery] int? categoryId, [FromQuery] int? tagId, [FromQuery] string? date)
        {
            var result = (await _articleRepository.GetAllArticlesAndDetailAsync());
            var intPageNum =pageNum??0;//pageNum根据前端接口来看必定传入,所以不用判断(不相信客户端传入的一切数据)
            //分类
            if (categoryId.HasValue)
            {
                result = result.Where(a => a.Category.Id == categoryId); 
            }
            //标签
            if (tagId.HasValue)
            {
                result = result.Where(result => result.Tags.Any(t => t.Id == tagId));
            }
            //日期
            if (!string.IsNullOrEmpty(date))
            {
                //取决于前端传入的格式
                var dateArr = date.Split("/");
                if (dateArr.Length == 1)
                {
                    result= result.Where(a => a.CreateTime.Year == Convert.ToInt32(dateArr[0]));
                }
                else if (dateArr.Length == 2)
                {
                    result = result.Where(a => a.CreateTime.Year == Convert.ToInt32(dateArr[0]) && a.CreateTime.Month == Convert.ToInt32(dateArr[1]));
                }
            }
            result = result.Skip((intPageNum-1)*pageSize).Take(pageSize);
            var articleList = _mapper.Map<IEnumerable<ArticleListVo>>(result);
            var PageVo = new PageVo<ArticleListVo>
            (
                articleList.Count(),
                articleList.ToList()
            );
            return new ResponseResult<PageVo<ArticleListVo>>(200,"操作成功",PageVo);
        }

        [HttpPut("/api/article/updateViewCount/{id}")]
        public async Task<ResponseResult<object>> UpdateArticleViewCount(int id)
        {
            var findresult = await _articleRepository.FindArticleByIdAsync(id);
            if (findresult==null)
            {
                return ResponseResult<object>.ErrorResult(code:404,msg:"找不到");
            }
            findresult.ViewIt();
            await _blogDbContext.SaveChangesAsync();
            return ResponseResult<object>.OkResult();
        }

        [HttpGet("/api/article/previousNextArticle/{id}")]
        public async Task<ResponseResult<PreviousNextArticleVo>> GetPreviousNextArticle(int id)
        {
            var next = id + 1;
            PreviousNextArticleVo previousNextArticleVo = new();
            var findResult = await _articleRepository.FindArticleByIdAsync(next);
            if (findResult==null)
            {
                previousNextArticleVo.next= null;
               // return ResponseResult<PreviousNextArticleVo>.ErrorResult(code:404,msg:"下一篇找不到");
            }
            previousNextArticleVo.next = _mapper.Map<HotArticleVo>(findResult);
            var previous = id - 1;
            var findResult1 = await _articleRepository.FindArticleByIdAsync(previous);
            if (findResult1==null)
            {
                previousNextArticleVo.previous = null;
               // return ResponseResult<PreviousNextArticleVo>.ErrorResult(code:404,msg:"上一篇找不到");
            }
            previousNextArticleVo.previous = _mapper.Map<HotArticleVo>(findResult1);
            return ResponseResult<PreviousNextArticleVo>.OkResult(data:previousNextArticleVo);
        }

        /// <summary>
        /// 发布文章
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("/api/article")]
        [Authorize]
        public async Task<ResponseResult<object>> AddArticle([FromHeader] string token,ArticleNewDTO articleNewDTO)
        {
            var user = await _identityDomainService.FIndUserByTokenAsync(token);
            var article = new Article(articleNewDTO.title, articleNewDTO.summary, articleNewDTO.content, articleNewDTO.thumbnail, articleNewDTO.isDraft, user);
            // 通过分类名找到分类
            var category = await _blogDbContext.Categories.FirstOrDefaultAsync(c => c.CategoryName == articleNewDTO.category);
            if (category != null)
            {
                
               article.ChangeType(category); 
            }
            else
            {
               //不存在则新建分类
               category = new Category(articleNewDTO.category);
                article.ChangeType(category);
            }
            //通过标签名找到标签
            var tagNames = articleNewDTO.tags;
            var tags = await _blogDbContext.Tags.Where(t => tagNames.Contains(t.TagName)).ToListAsync();
            article = article.ChangeTags(tags);
            await _blogDbContext.AddAsync(article);
            await _blogDbContext.SaveChangesAsync();
            return new ResponseResult<object>(200,"SUCCESS",article.Id);//返回一个id给前端重定向到对应页面
        }
        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="token"></param>
        /// <param name="articleEditDTO"></param>
        /// <returns></returns>
        [HttpPut("/api/article")]
        [Authorize]
        public async Task<ResponseResult<object>> UpdateArticle([FromHeader] string token, ArticleEditDTO articleEditDTO)
        {
            if(token==null)
            {
                return new ResponseResult<object>(401, "没有权限");
            }

            var user = await _identityDomainService.FIndUserByTokenAsync(token);
            if (user==null)
            {
                return new ResponseResult<object>(401, "请登录");
            }

            //判断用户是不是管理员
            var IsAdmin = await _identityDomainService.IsAdminAsync(user);
            if (!IsAdmin)
            {
                return new ResponseResult<object>(401, "没有权限");
            }
            //包含相关信息的文章
            var article = await _articleRepository.FindArticleByIdIncludeAsync(articleEditDTO.Id);
            if (article == null)
            {
                return new ResponseResult<object>(404, "找不到该文章");
            }
            if (article.AuthorName != user.UserName)
            {
                //只能修改自己的文章
                return new ResponseResult<object>(401, "只能修改自己的文章");
            }
            // 修改并保存文章,以后再想怎么直接使用automapper保存一个复杂对象吧

            var category = await _blogDbContext.Categories.FirstOrDefaultAsync(c => c.CategoryName == articleEditDTO.category);
            if (category==null)
            {
                category =new Category(articleEditDTO.category);
              //  return new ResponseResult<object>(404, "找不到该分类");
            }
            var tagNames = articleEditDTO.tags;
            var tags = new List<Tag>();
            //如果标签不存在,则创建标签

            //修改文章
            //_blogDbContext.RemoveRange(article.Tags);
            
            //这样输入重复标签会有问题...
            foreach (var tagName in tagNames)
            {
                var tag = await _blogDbContext.Tags.FirstOrDefaultAsync(t => t.TagName == tagName);
                if (tag == null)
                {
                    tag = new Tag(tagName);
                    _blogDbContext.Tags.Add(tag);
                }
                tags.Add(tag);
            }
            article.Tags.Clear();
            article = article.ChangeTags(tags);//tags...重复插入键要先清除
            
            article = article.ChangeTitle(articleEditDTO.title);//title
           // _blogDbContext.Remove(article.Category);
            article = article.ChangeType(category);//category            
            article = article.ChangeSummary(articleEditDTO.summary);//summary
            article.IsDraft = articleEditDTO.isDraft;//isDraft
            article.Content = articleEditDTO.content;//content
            article.Thumbnail = articleEditDTO.thumbnail;//thumbnail
            //保存文章
            _blogDbContext.Update(article);
            await _blogDbContext.SaveChangesAsync();
            return new ResponseResult<object>(200,"成功",article.Id);
        }

        [HttpPost("/api/AddNewArticle")]
        [UnitOfWork(typeof(BlogDbContext))]
        [Obsolete("这个方法只在测试静态文件时使用")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PublishArticle([FromForm] AddNewArticleRequest article, IFormFile formFile)
        {
            var user = await _identityRepository.FindUserByNameAsync("Admin");//这样写只要是管理员角色写的博客都是显示管理员用户
            var newArticle = new Article(article.Title, article.Describe, article.Path, user);
            if (formFile.Length > 0)
            {
                if (formFile.FileName.EndsWith(".md"))//这段能不能改？
                {
                    var filePath = Path.Combine(_config,article.Path,formFile.FileName);//保存一份到文件系统,Path为自定义
                    string targetDirectoryPath = Path.Combine(_config, article.Path);
                    Directory.CreateDirectory(targetDirectoryPath);
                    using (var fileStream = System.IO.File.Create(filePath))
                    {
                        byte[] buffer;//不要设置为null,不用赋值都行
                         // 1. 读取文件内容到缓冲区
                        using (var memoryStream = new MemoryStream())
                        {
                            await formFile?.CopyToAsync(memoryStream);
                            buffer = memoryStream.ToArray();
                        }

                        // 2. 将文件内容写入文件流
                        await fileStream.WriteAsync(buffer);
                        await fileStream.FlushAsync();

                        // 3. 重置文件流的位置
                        fileStream.Position = 0;

                        // 4. 设置新文章的内容
                        newArticle.Content = Encoding.UTF8.GetString(buffer);

                        // 5. 保存新文章到数据库
                        await _articleRepository.AddArticleAsync(newArticle);
                        
                    }
                }
            }
            //应用了UnitOfWork特性,这里不需要保存
            return Ok("测试的添加成功:"+newArticle.Content);//怎么判断是否添加成功
        }


    }
}
