using AutoMapper;
using BackEndWebAPI.Controllers.IdentityController;
using BackEndWebAPI.DTO;
using BackEndWebAPI.VO;
using BackEndWebAPI.WebAPIExtensions;
using Domain;
using Domain.Entities;
using Infrasturacture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace BackEndWebAPI.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController :ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _IdentityRepository;
        private readonly IdentityDomainService _identityDomainService;
        private readonly BlogDbContext _blogDbContext;

        public UserController(IMapper mapper, IIdentityRepository identityRepository, IdentityDomainService identityDomainService, BlogDbContext blogDbContext)
        {
            _mapper = mapper;
            _IdentityRepository = identityRepository;
            _identityDomainService = identityDomainService;
            _blogDbContext = blogDbContext;
        }

        // 获取管理员信息
        [HttpGet("/api/user/adminInfo")]
       // [EnableCors]

        public async Task<ResponseResult<UserShownInfoVO>> GetAdminInfo()
        {
            var admin = await _IdentityRepository.FindUserByNameAsync("Admin"); //使用Async方法需要await
            if (admin == null) { return new ResponseResult<UserShownInfoVO>(404, "字符串是不是配错了,不应该啊"); }
            var adminInfoVo = _mapper.Map<UserShownInfoVO>(admin);
            return new ResponseResult<UserShownInfoVO>(200, "操作成功", adminInfoVo);
        }
        //获取用户信息
        [HttpGet("/api/user/userInfo")]
        public async Task<ResponseResult<UserShownInfoVO>> GetUserInfo([FromHeader]string token)
        {
            var userInfoVo = await GetUserShownInfoAsync(token);
            if (userInfoVo == null) { return new ResponseResult<UserShownInfoVO>(404, "Token无效"); }
            return new ResponseResult<UserShownInfoVO>(200, "操作成功", userInfoVo);
        }

        //注册用户
        //不封装请求,直接用参数,这样就不用写一个类了,SwaggerUI上测试也方便,但是无法使用FluentValadation
        [HttpPost("/api/user/register")]
        public async Task<ResponseResult<string>> RegisterByUserAndPwd(RegisterRequest req)
        {
            var result = await _IdentityRepository.CreateUserDefaultAsync(req.userName, req.nickName, req.email, req.password);
            if (result.Succeeded)
            {
                return new ResponseResult<string>(code: 200, msg: "操作成功");
            }
            else { return new ResponseResult<string>(code: 400, msg: "失败:" + result); }
        }
        // 用户信息更新,不涉及密码
        [HttpPut("/api/user/userInfo")]
        [Authorize]
        public async Task<ResponseResult<object>> UpdateUserInfo(UserDTO userDTO)
        {
            var user = await _IdentityRepository.FindUserByIdAsync(userDTO.id.ToString());
            if (user == null)
            {
                return ResponseResult<object>.ErrorResult(404, "用户不存在");
            }
            // 直接在控制器层这样访问不好
            user.PhoneNumber = userDTO.phonenumber; user.UserName = userDTO.userName;
            user.Email = userDTO.email; user.NickName = userDTO.nickName;
            user.avatar = userDTO.avatar; user.signature = userDTO.signature;
            user.sex = userDTO.sex; 
            //user.NormalizedUserName = userDTO.userName.Normalize();
            //user.NormalizedEmail = userDTO.email.Normalize();
            //如下代码会报错,思考如何在这种情况下使用AutoMapper
            // user =_mapper.Map<UserDTO, User>(userDTO); 
            _blogDbContext.Update(user);
            await _blogDbContext.SaveChangesAsync();
            return new ResponseResult<object>(200, msg: "操作成功");
        }
        private async Task<UserShownInfoVO> GetUserShownInfoAsync(string token)
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
            var user = await _identityDomainService.FindUserByIdAsync(userId);//直接用userManager更方便
            if (user == null) { await Console.Out.WriteLineAsync("没找到用户"); }
            var userShownInfoVO = _mapper.Map<UserShownInfoVO>(user);
            return userShownInfoVO;
        }
    }
}
