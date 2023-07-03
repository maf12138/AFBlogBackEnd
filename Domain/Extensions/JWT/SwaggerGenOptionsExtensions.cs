using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Domain.Extensions.JWT
{

    /// <summary>
    /// 复制粘贴的一段代码
    /// 为Swagger增加Authentication报文头
    /// </summary>
    /// <param name="c"></param>
    public static class SwaggerGenOptionsExtensions
    {

        public static void AddAuthenticationHeader(this SwaggerGenOptions c)
        {
            c.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme
            {
                Description = "Authorization header. \r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Authorization"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Authorization"
                        },
                        Scheme = "oauth2",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        }
    }
}
