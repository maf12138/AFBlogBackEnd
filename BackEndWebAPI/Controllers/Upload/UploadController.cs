using BackEndWebAPI.Configs;
using BackEndWebAPI.WebAPIExtensions;
using Infrasturacture.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;
using System.Buffers.Text;
using System.Text;

namespace BackEndWebAPI.Controllers.Upload;

[Route("api/[controller]")]
[ApiController]
public class UploadController : ControllerBase
{
    private readonly ILogger<System.IO.FileInfo> _logger;
    private readonly string _config;
    private readonly IOptionsSnapshot<QiNiuOptions> _optionsSnapshot;

    public UploadController(ILogger<System.IO.FileInfo> logger,IOptionsSnapshot<QiNiuOptions> optionsSnapshot)
    {
        _logger = logger;
        _optionsSnapshot = optionsSnapshot;
        _config = Directory.GetCurrentDirectory() + "\\docs";
    }

/*    private async Task<string> GetCDNName()
    {
        return  await Task.FromResult(_optionsSnapshot.Value.CDNName);
    }*/

    //返回上传成功的URL
    [HttpPost("/api/image/upload")]
    public async Task<ResponseResult<string>> UploadFromFile(IFormFile file)
    {
        var token = await GetUploadToken();//获取上传凭证
        var config = await GetConfigAsync();//获取上传配置
        var uploadManager = new UploadManager(config);
        var key =file.FileName;//文件名-考虑覆盖问题?
        using(var stream = file.OpenReadStream())
        {
            var result =  uploadManager.UploadStream(stream, key, token, null);
            if (result != null && result.Code == 200)
            {
                // 上传成功，返回文件的URL,在text的json的key中
                var jsonObject = JsonConvert.DeserializeObject<JObject>(result.Text);
                var fileUrl = jsonObject["key"].ToString();
                return ResponseResult<string>.OkResult("https://httpscdn.aofei.site/" + fileUrl);
            }
            return ResponseResult<string>.ErrorResult(result.Code,"上传失败");
        }
      //  FormUploader target = new FormUploader(config);
      //  HttpResult result = target.UploadFile(filePath, key, token, null);//官网文档是本地文件路径...
    }
    private async Task<Config> GetConfigAsync()
    {
        Config config = new Config();
        // 设置上传区域
        config.Zone = Zone.ZONE_CN_South;//华南-广东
        // 设置 http 或者 https 上传
        config.UseHttps = true;//需要在云上配置
        config.UseCdnDomains = true;
        return config;
    }

    // 目前只用于图片的上传
    private async Task<string>  GetUploadToken()
    {
        string  AccessKey = _optionsSnapshot.Value.AccessKey;
        string  SecretKey = _optionsSnapshot.Value.SecretKey;
        Mac mac = new Mac(AccessKey, SecretKey);
        // 设置上传策略，详见：https://developer.qiniu.com/kodo/manual/1206/put-policy
        PutPolicy putPolicy = new PutPolicy();
        var Bucket = "mafweb";
        putPolicy.Scope = Bucket;
        putPolicy.SetExpires(3600);//设置凭证过期时间
        putPolicy.DeleteAfterDays = 1;
        putPolicy.MimeLimit = "image/*";//设置只允许图片上传
        putPolicy.FsizeLimit = 10485760;//1024*1024*10 byte == 10mb 
        string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());//生成上传凭证
        await Console.Out.WriteLineAsync(token);
        return token;
    }
    /// <summary>
    /// 上传文件到服务器目录
    /// </summary>
    /// <param name="files">文件</param>
    /// <returns></returns>
    /// 
    [HttpPost("服务器直传")]
    [Obsolete("没什么用..")]
    public ActionResult Upload(IFormFile file)
    {
        try
        {
            // 服务器将要存储文件的路径
            var root = Directory.GetCurrentDirectory();//应用程序当前路径(执行dotnet命令的目录)
            var Folder = Path.Combine(root, "/maftestfile");

            if (Directory.Exists(Folder) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(Folder);
            }
            //https://learn.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.http.iformfilecollection?view=aspnetcore-7.0  
            StreamReader reader = new StreamReader(file.OpenReadStream());
            var content = reader.ReadToEnd();
            var name = file.FileName; // 获取文件名
            string pathName = Path.GetExtension(name); // 获取后缀名
            if (pathName != ".md")
            {
                return BadRequest("不支持的格式");
            }

            String filename = Folder + Guid.NewGuid().ToString().Replace("-", "") + pathName; // 生成新的文件名，唯一不重复
            if (System.IO.File.Exists(filename))//删除重名
            {
                System.IO.File.Delete(filename);
            }
            using (FileStream fs = System.IO.File.Create(filename))
            {
                // 复制文件
                file.CopyTo(fs);
                // 清空缓冲区数据
                fs.Flush();
            }

            return Ok("文件上传成功");
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}
