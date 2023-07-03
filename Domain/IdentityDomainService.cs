using AutoMapper;
using Domain.Entities;
using Domain.Extensions.JWT;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Domain
{
    /// <summary>
    /// 登录,注册,改密码,查看信息,注销,等业务逻辑
    /// 应用服务的话调用领域服务去完成用例
    /// </summary>
    public class IdentityDomainService
    {
        private readonly IIdentityRepository _IdentityRepository;
        private readonly ITokenService _tokenService;
        private readonly IOptions<JWTOptions> _JWTOptions;
        private readonly IMapper _mapper;
        //private readonly ISmsCodeSender _smsCodeSender;

        public IdentityDomainService(IIdentityRepository userRepository, ITokenService tokenService, IOptions<JWTOptions> options, IMapper mapper)
        {
            _IdentityRepository = userRepository;
            _tokenService = tokenService;
            _JWTOptions = options;
            _mapper = mapper;
        }
        public async Task<User> FindUserByIdAsync(string id)
        {
            return await _IdentityRepository.FindUserByIdAsync(id);
        }
        /// <summary>
        /// 检查用户名和密码并返回检查结果
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<SignInResult> CheckUserNameAndPwdAsync(string userName, string password)
        {
            var user = await _IdentityRepository.FindUserByNameAsync(userName);
            if (user == null) { return SignInResult.Failed; }
            var result = await _IdentityRepository.CheckForSignIn(user, password);
            return result;
        }

        /// <summary>
        /// 发Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<string> BuildTokenAsync(User user)
        {
            var roles = await _IdentityRepository.GetRolesAsync(user);
            List<Claim> clams = new List<Claim>();
            // NameIdentifier 和 Name 区别?
            // //将用户id加入claims
            clams.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            //将用户角色加入claims
            //有的用户没有角色怎么办?可空的
            foreach (var role in roles)
            {
                clams.Add(new Claim((ClaimTypes.Role), role));
            }
            //将用户信息加入claims
            clams.Add(new Claim(ClaimTypes.Name, user.UserName));
            //解决jwt撤回问题,还要在 登录?/退出?/ 软删除/改密码等操作时更改user的jwt信息,可以改用redis或accesstoken+refreshtoken方式
            clams.Add(new Claim(ClaimTypes.Version,user.JWTVersion.ToString()));
            // 下面两个属于是敏感信息了,而且也不利于测试
           // clams.Add(new Claim(ClaimTypes.Email, user.Email));
           //clams.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
            return _tokenService.BuildToken(clams, _JWTOptions.Value);
        }

        /// <summary>
        /// 登录逻辑
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<(SignInResult Result, string? Token)> LoginByUserNameAndPwdAsync(string userName, string password)
        {
            var result = await CheckUserNameAndPwdAsync(userName, password);
            if (result.Succeeded)
            {
                var user = await _IdentityRepository.FindUserByNameAsync(userName);
                var token = await BuildTokenAsync(user);
                return (SignInResult.Success, token);
            }
            else
            {
                return (result, null);
            }

        }
        // 获取用户是否为管理员角色
        public async Task<bool> IsAdminAsync(User user)
        {
            var userRoles = await _IdentityRepository.GetRolesAsync(user);
            var isAdmin = userRoles.Contains("Admin");
            return isAdmin;
        }
        

        public async Task<User> FIndUserByTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) { await Console.Out.WriteLineAsync("token为空"); }
            var userId = new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.First().Value;
            var user = await FindUserByIdAsync(userId);//直接用userManager更方便
            if (user == null)
            {
                await Console.Out.WriteLineAsync("没找到用户");
                return user;
            }
            return user;

        }


    }
}
