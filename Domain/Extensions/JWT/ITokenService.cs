using System.Security.Claims;

namespace Domain.Extensions.JWT
{
    public interface ITokenService
    {
        /// <summary>
        /// 传入含有用户信息的claims,不要携带密码等关键信息
        /// 传入程序配置的JWTOptions对象,用于生成JWT(这里的JWTOptions对象是在程序启动时注入的)//括号是Copilot加的
        /// </summary>
        /// <param name="claims">用户信息</param>
        /// <param name="options">程序配置</param>
        /// <returns>JWT</returns>
        public string BuildToken(IEnumerable<Claim> claims, JWTOptions options);
    }

}
