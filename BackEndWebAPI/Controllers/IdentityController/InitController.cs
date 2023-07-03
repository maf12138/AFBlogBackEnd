using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BackEndWebAPI.Controllers.IdentityController
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitController : ControllerBase
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IConfiguration _configuration;

        public InitController(IIdentityRepository identityRepository, IConfiguration configuration)
        {
            _identityRepository = identityRepository;
            _configuration = configuration;
        }
        //初始化完就可以删了/修改了,应该自定义
        [HttpPost]
        public async Task<IActionResult> Init(string pwd)
        {
            if (!pwd.IsNullOrEmpty() && pwd == "666666")
            {
                //判断是否已经初始化过了
                if (_configuration.GetSection("InitCount:Count").Value == "0")
                {
                    _configuration.GetSection("InitCount:Count").Value = "1";
                    var result = await _identityRepository.CreateUserByUserNameAndPwdAsync("Admin", "666666");
                    if (result.Succeeded)
                    {
                        var roleCreateResult = await _identityRepository.CreateRole("Admin");
                        if (roleCreateResult.Succeeded)
                        {
                            var user = await _identityRepository.FindUserByNameAsync("Admin");
                            var addResult = await _identityRepository.AddRoleToUserAsync(user, "Admin");
                            if (addResult.Succeeded)
                            {
                                return Ok("初始化成功");
                            }
                            else
                            { return BadRequest("添加角色到用户失败"); }
                        }
                        else { return BadRequest("创建管理员角色失败，请检查数据库或者代码"); }
                    }
                    else
                    {
                        return BadRequest("创建用户失败");
                    }
                }
                else
                {
                    return BadRequest("已经初始化过了");
                }

            }
            else
            {
                return BadRequest("密码错误");
            }
        }
    }
}
