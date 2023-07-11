using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace BackEndWebAPI.Filters;
public class RateLimitFilter : IAsyncActionFilter
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<RateLimitFilter> _logger;
    //https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.caching.memory?view=dotnet-plat-ext-7.0
    public RateLimitFilter(IMemoryCache memoryCache, ILogger<RateLimitFilter> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        int count = 100;//每10秒允许的请求次数次
        var remoteIp = context.HttpContext.Connection.RemoteIpAddress;//如果使用Nginxf反向代理, 这里获取的是Nginx的IP,需要在Nginx配置文件中增加: proxy_set_header X-Real-IP $remote_addr;并且启用中间件: app.UseForwardedHeaders();
        var cacheKey = $"RateLimitFilter.{remoteIp}";
        var cacheEntry = _memoryCache.GetOrCreate(cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);//设置缓存过期时间
            entry.SetPriority(CacheItemPriority.Normal);
            return 0;
        });//获取缓存,如果没有就创建一个
        if (cacheEntry >= count)
        {
            _logger.LogWarning($"RateLimitFilter: {remoteIp} has been limited");
            context.Result = new StatusCodeResult((int)HttpStatusCode.TooManyRequests);
            // context.Result  = new ContentResult { Content = "TooManyRequests", StatusCode = (int)HttpStatusCode.TooManyRequests };
            return Task.CompletedTask;
        }
        else
        {
            cacheEntry++;
            _memoryCache.Set(cacheKey, cacheEntry, TimeSpan.FromSeconds(10));
            return next();
        }
    }

    /*  public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
      {
          var remoteIp = context.HttpContext.Connection.RemoteIpAddress;//如果使用Nginxf反向代理, 这里获取的是Nginx的IP,需要在Nginx配置文件中增加: proxy_set_header X-Real-IP $remote_addr;并且启用中间件: app.UseForwardedHeaders();
          var cacheKey = $"RateLimitFilter.{remoteIp}";
          long? @long = _memoryCache.Get<long?>(cacheKey);//@+关键字 语法是C#7的语法, 用来避免关键字冲突
          if (@long == null || Environment.TickCount64 - @long > 1000)
          {
              _memoryCache.Set(cacheKey, Environment.TickCount64, TimeSpan.FromSeconds(10));
              return next();
          }
          else
          {
              _logger.LogWarning($"RateLimitFilter: {remoteIp} has been limited");
              context.Result = new StatusCodeResult((int)HttpStatusCode.TooManyRequests);
              return Task.CompletedTask;
          }
      }*/
}
