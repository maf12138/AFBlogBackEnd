using Domain;
using Domain.Entities;
using Domain.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Infrasturacture
{
    public class IdentityRepository : IIdentityRepository
    {
        //这两个Manager的操作都有失败的可能性,需要处理失败逻辑,还有个SIgnManager有待了解
        //增加,删除,修改,查找 等操作都有失败的可能性,返回值按需设置为IdentityResult
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public IdentityRepository(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// 用于IdentityError的返回
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static IdentityResult ErrorResult(string msg)
        {
            IdentityError idError = new IdentityError { Description = msg };
            return IdentityResult.Failed(idError);
        }

        public async Task<IdentityResult> AddRoleToUserAsync(User user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                return IdentityResult.Success;
            }
            else
            {
                return ErrorResult("添加角色失败");
            }
        }

        public async Task<IdentityResult> ChangePwdAsync(string name, string PWD, string newPWD)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {
                return ErrorResult("查无此人");
            }
            else
            {
                return await _userManager.ChangePasswordAsync(user, PWD, newPWD);
            }
        }
        /// <summary>
        /// 用SignInResult规定返回
        /// 成功,失败,锁定,不允许,双因素认证...
        /// 强制进行失败锁定
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<SignInResult> CheckForSignIn(User user, string password)
        {
            //检查是否锁定
            if (await _userManager.IsLockedOutAsync(user))
            {
                return SignInResult.LockedOut;
            }
            //获取密码匹配结果
            var succcess = await _userManager.CheckPasswordAsync(user, password);
            if (succcess)
            {
                return SignInResult.Success;
            }
            else
            {
                //失败计数
                var result = await _userManager.AccessFailedAsync(user);
                if (!result.Succeeded)
                {
                    //throw new Exception("密码失败计数失败");  //自动补全的...
                    throw new ApplicationException("密码失败计数失败");
                }
                return SignInResult.Failed;
            }
        }

        public async Task<IdentityResult> ConfirmEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("查无此人");//用异常会怎么样?
            }
            else
            {
                return await _userManager.ConfirmEmailAsync(user, email);
            }
        }

        public async Task<IdentityResult> CreateRole(string name)
        {
            var roleExists = await _roleManager.RoleExistsAsync(name);
            if (roleExists)
            {
                throw new Exception("已有重名角色");
            }
            else
            {
                var result = await _roleManager.CreateAsync(new Role { Name = name });
                if (!result.Succeeded)
                {
                    return ErrorResult("角色创建失败");
                }
                return result;
            }

        }
        /// <summary>
        /// 此处逻辑可移到service
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateUserByEmail(string email, string password, string userName)
        {
            var isExits = await _userManager.FindByEmailAsync(email);
            if (isExits != null)
            {
                return ErrorResult("邮箱已使用");
            }
            var isNameUsed = await _userManager.FindByNameAsync(userName);
            if (isNameUsed != null)
            {
                return ErrorResult("用户名已被使用");
            }
            var randomName = email.Split('@')[0];
            var user = new User(userName);
            user.Email = email;
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return ErrorResult("用户创建失败");
            }
            return result;
        }

        public async Task<IdentityResult> CreateUserByUserNameAndPwdAsync(string name, string pwd)
        {
            //不能重名
            var user = await FindUserByNameAsync(name);
            if (user == null)
            {
                var result = await _userManager.CreateAsync(new User(name), pwd);
                if (result.Succeeded) { return IdentityResult.Success; }
                else
                {
                    return result;
                }

            }
            return ErrorResult("已有重名用户");

        }
        public async Task<IdentityResult> CreateUserDefaultAsync(string name, string nickname, string email, string pwd)
        {
            var user = await FindUserByNameAsync(name);
            if (user == null)
            {
                var result = await _userManager.CreateAsync(new User(name), pwd);
                if (result.Succeeded)
                {
                    var newUser = await FindUserByNameAsync(name);
                    newUser.Email = email;
                    newUser.NickName = nickname;//这样写不利于维护,应该用方法保证一致性
                    await _userManager.UpdateAsync(newUser);
                    return IdentityResult.Success;
                }
                else
                {
                    return result;
                }
            }
            return ErrorResult("已有重名用户");

        }



        public async Task<IdentityResult> DeleteUserfAsync(string id)
        {
            var usr = await _userManager.FindByIdAsync(id.ToString());
            if (usr != null)
            {
                var result = await _userManager.DeleteAsync(usr);
                if (!result.Succeeded)
                {
                    return ErrorResult("删除失败");
                }
                return result;
            }
            return ErrorResult("用户不存在");
        }

        public Task<Role> FindRoleByNameAsync(string name)
        {
            var role = _roleManager.FindByNameAsync(name);
            if (role == null)
            {
                throw new Exception("查无此角色");
            }
            return role;
        }

        public Task<User> FindUserByEmailAsync(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }

        public Task<User> FindUserByIdAsync(string id)
        {
            return _userManager.FindByIdAsync(id);
        }

        public Task<User> FindUserByNameAsync(string name)
        {
            return _userManager.FindByNameAsync(name);
        }
        /// <summary>
        /// 没有发送功能只是返回了Token...
        /// 还可以用来设置初始密码之类的
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<string> GenerateTokenByEmail(string email)
        {
            var usr = await FindUserByEmailAsync(email);
            if (usr != null)
            {
                var token = IdentityHelper.GenerateToken(6);
                return token;
            }
            else
                return ErrorResult("未找到邮箱").ToString();


        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<(IdentityResult, string? password)> ResetPwdAsync(string id)
        {
            var usr = await FindUserByIdAsync(id);
            if (usr != null)
            {
                string pwd = IdentityHelper.GenerateToken(6);
                string token = await _userManager.GeneratePasswordResetTokenAsync(usr);
                var result = await _userManager.ChangePasswordAsync(usr, pwd, token);
                if (!result.Succeeded)
                {
                    return (ErrorResult("密码重置失败"), null);
                }
                return (IdentityResult.Success, pwd);//第一个返回值可以直接返回result ,有什么区别呢
            }
            else
            {
                return (ErrorResult("未找到用户"), null);
            }
        }

        public async Task<IdentityResult> UpdateEmialAsync(string id, string emial)
        {
            var usr = FindUserByIdAsync(id);
            if (usr != null)
            {
                return await _userManager.SetEmailAsync(usr.Result, emial);
            }
            else
            {
                return await Task.FromResult(ErrorResult("未找到用户"));//自动补全式写法
            }
        }
    }
}
