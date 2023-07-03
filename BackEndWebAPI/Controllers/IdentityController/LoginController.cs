using AutoMapper;
using BackEndWebAPI.DTO;
using BackEndWebAPI.VO;
using BackEndWebAPI.WebAPIExtensions;
using Domain;
using Infrasturacture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;


namespace BackEndWebAPI.Controllers.IdentityController
{
    //CQRS模型，与增删改不同的应用服务，是查询应用服务。不必遵守DDD分层规则（不会对数据做修改）。简单逻辑甚至可以直接由controller层调用仓储层返回数据
    //元组无法推断复杂类型,也就是要么匿名类型要么使用封装好的类
    [Route("api/[controller]")]
    [ApiController]
    public partial class LoginController : ControllerBase
    {
        private readonly IIdentityRepository _IdentityRepository;
        private readonly IdentityDomainService _IdentityDomainService;
        private readonly IMapper _mapper;
        private readonly BlogDbContext _blogDbContext;
        public LoginController(IdentityDomainService identityDomainService, IIdentityRepository identityRepository, IMapper mapper, BlogDbContext blogDbContext)
        {
            _IdentityDomainService = identityDomainService;
            _IdentityRepository = identityRepository;
            _mapper = mapper;
            _blogDbContext = blogDbContext;
        }
        /// <summary>
        /// 通过用户名密码登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("/api/login")]
        public async Task<ResponseResult<BlogUserLoginVo>> LoginByUserNameAndPwd(LoginByUserNameAndPwdRequest req)
        {
            //自动补全出大坑,req没有username属性但是补全上去了没发现有什么不一样
            (var signResult, var token) = await _IdentityDomainService.LoginByUserNameAndPwdAsync(req.userName, req.password);
            if (signResult.Succeeded)
            {
                //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-forgiving 关于!
                var userInfo = await GetUserInfoAsync(token);//根据条件必不为空
                BlogUserLoginVo result = new BlogUserLoginVo(token, userInfo);
                return new ResponseResult<BlogUserLoginVo>(200, "操作成功", result);
            }
            else if (signResult.IsLockedOut)
            {
                //HttpStatusCode帮助更好的实现Restful,是一种约束 ,Locked=423
                return ResponseResult<BlogUserLoginVo>.ErrorResult(423, "用户锁定了");
            }
            else
            {
                string msg = signResult.ToString();
                return ResponseResult<BlogUserLoginVo>.ErrorResult(666, "查看详细:" + msg);
            }
        }

        [HttpPost("/api/logout")]
        [Authorize]
        public async Task<ResponseResult<object>> LogOut([FromHeader] string token)
        {
            //清除服务器token,收回操作...
            if (token == null) { return new ResponseResult<object>().Error(401, "需要登录之后才能操作"); }
            var user = await _IdentityDomainService.FIndUserByTokenAsync(token);
            if (user == null) { return new ResponseResult<object>().Error(404, "token无效,请登录之后在尝试"); }
            user.JWTVersion++;
            _blogDbContext.Update(user);
            await _blogDbContext.SaveChangesAsync();
            //应该让客户端清除token
            
            return ResponseResult<object>.OkResult();
        }

        /// <summary>
        /// 多加一层真的麻烦不如直接调用仓储
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        [HttpPost("/api/更改密码")]
        [Authorize]
        public async Task<ActionResult<string>> ChangePwdByOlder(ChangePwdRequest req)
        {
            var result = await _IdentityRepository.ChangePwdAsync(req.Uid, req.Pwd, req.NewPwd);
            
            if (result.Succeeded)
            {
                var user = await _IdentityRepository.FindUserByIdAsync(req.Uid);
                user.JWTVersion++;
                _blogDbContext.Update(user);
                await _blogDbContext.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(result.ToString());
            }
        }
        //不封装请求,直接用参数,这样就不用写一个类了,SwaggerUI上测试也方便
/*        [HttpPost("/user/register")]
        public async Task<ResponseResult<string>> RegisterByUserAndPwd(RegisterRequest req)
        {
            var result = await _IdentityRepository.CreateUserDefaultAsync(req.userName, req.nickName, req.email,req.password);           
            if(result.Succeeded)
            {                
                return new ResponseResult<string>(code:200,msg:"操作成功");
            }
            else { return new ResponseResult<string>(code: 400, msg: "失败:" + result); }
        }
*/



        //根据token获取用户信息
        private async Task<UserInfoVo> GetUserInfoAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {             
                await Console.Out.WriteLineAsync("token为空");
            }
            /*  // 提取用户信息
            var usernameClaim = tokenS.Claims.FirstOrDefault(c => c.Type == "username");
            var emailClaim = tokenS.Claims.FirstOrDefault(c => c.Type == "email");
            var nameClaim = tokenS.Claims.FirstOrDefault(c => c.Type == "name");
            */
            var userId = new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.First().Value;
            var user = await _IdentityDomainService.FindUserByIdAsync(userId);//直接用userManager更方便
            if (user == null) { await Console.Out.WriteLineAsync("没找到用户"); }
            var userInfoVo = _mapper.Map<UserInfoVo>(user);
            return userInfoVo;
        }
    }
}
