using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Domain.Extensions.JWT
{

    public static class AuthenticationExtensions
    {
        /// <summary>
        /// 通过扩展方法的语法进行服务注入
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddJWTAuthentication(this IServiceCollection services, JWTOptions options)
        {
            return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
            {
                //配置程序jwt的验证
                x.TokenValidationParameters = new()
                {
                    //启用哪些验证
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //令牌的发送方
                    ValidIssuer = options.Issuer,
                    //令牌可以用在哪些域名上
                    ValidAudience = options.Audience,
                    //配置验证
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key))
                };


            });
        }
    }
}
