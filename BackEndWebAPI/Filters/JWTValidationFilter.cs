using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Security.Claims;

namespace BackEndWebAPI.Filters
{
    public class JWTValidationFilter : IAsyncActionFilter
    {
        private readonly IMemoryCache memoryCache;
        private readonly UserManager<User> userManager;

        public JWTValidationFilter(IMemoryCache memoryCache, UserManager<User> userManager)
        {
            this.memoryCache = memoryCache;
            this.userManager = userManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var claimUserId =context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);//查看有无token
            if (claimUserId == null) { await next(); return; }//没有就传递给下个
            Guid userId= Guid.Parse(claimUserId.Value);
            string cacheKey = $"JWTValidationFilter.UserInfo.{userId}";//增加了调试可读性...
            User user = await memoryCache.GetOrCreateAsync(cacheKey, async e =>
            {
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);
                return await userManager.FindByIdAsync(userId.ToString());
            });
            if (user == null)
            {
                var result = new ObjectResult($"UserId({userId}) not found");
                result.StatusCode = (int)HttpStatusCode.Unauthorized;//这里直接设置了StatusCode
                context.Result = result;
                return;
            }
            var claimVersion = context.HttpContext.User.FindFirst(ClaimTypes.Version);
            long jwtVerOfReq = long.Parse(claimVersion!.Value);
            if (jwtVerOfReq >= user.JWTVersion)
            {
                await next();
            }
            else
            {
                var result = new ObjectResult($"JWTVersion mismatch");
                result.StatusCode = (int)HttpStatusCode.Unauthorized;//同上
                context.Result = result;
                return;
            }
            
        }
    }
}
