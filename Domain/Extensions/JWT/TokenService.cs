using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Domain.Extensions.JWT
{
    public class TokenService : ITokenService
    {
        /// <summary>
        /// 传入含有用户信息的claims,不要携带密码等关键信息
        /// 传入程序配置的JWTOptions对象,用于生成JWT(这里的JWTOptions对象是在程序启动时注入的)//括号是Copilot加的
        /// </summary>
        /// <param name="claims">用户信息</param>
        /// <param name="options">程序配置</param>
        /// <returns>JWT</returns>
        public string BuildToken(IEnumerable<Claim> claims, JWTOptions options)
        {
            //SymmetricSecurityKey,SigningCredentials,SecurityAlgorithms 均属Microsoft.IdentityModel.Tokens包
            //JwtSecurityToken属于System.IdentityModel.Tokens.Jwt;这个包依赖了上面的包,所以可以只装这个
            //由于expires参数属于datatime,即在指定的日期过后过期,所以可以用now()当前日期加上过期时间进行判定
            TimeSpan ExpiryDuration = TimeSpan.FromSeconds(options.ExpireSeconds);
            var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);
            // 跳过了一个可空参数,从跳过后面的参数都是显式指定的,这个方法有很多重载
            var tokenDescriptor = new JwtSecurityToken(options.Issuer, options.Audience, claims, expires: DateTime.Now.Add(ExpiryDuration), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }

}
