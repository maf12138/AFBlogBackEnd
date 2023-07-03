using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public interface IIdentityRepository
    {
        public Task<User> FindUserByIdAsync(string id);//通过id查找用户
        /// <summary>
        /// 通过用户名查找用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<User> FindUserByNameAsync(string name);
        /// <summary>
        /// 通过邮箱查找用户
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<User> FindUserByEmailAsync(string email);
        /// <summary>
        /// 通过邮箱和密码创建用户
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<IdentityResult> CreateUserByEmail(string email, string password, string userName);
        /// <summary>
        /// 通过用户名和密码创建用户
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<IdentityResult> CreateUserByUserNameAndPwdAsync(string name, string pwd);
        /// <summary>
        /// 通过用户名和旧密码更改密码
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="PWD">旧密码</param>
        /// <param name="newPWD">新密码</param>
        /// <returns></returns>
        public Task<IdentityResult> ChangePwdAsync(string name, string PWD, string newPWD);
        /// <summary>
        /// 通过邮箱生成Token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<string> GenerateTokenByEmail(string email);
        /// <summary>
        /// 用户登录检查
        /// </summary>
        /// <param name="user"> 考虑用string 类型而不是用户类型</param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<SignInResult> CheckForSignIn(User user, string password);
        /// <summary>
        /// 确认用户邮箱
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<IdentityResult> ConfirmEmail(string email);
        /// <summary>
        /// 用户注册默认使用的参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nickname"></param>
        /// <param name="email"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public Task<IdentityResult> CreateUserDefaultAsync(string name, string nickname, string email, string pwd);
        /// <summary>
        /// 修改邮箱
        /// </summary>
        /// <param name="id"></param>
        /// <param name="emial"></param>
        /// <returns></returns>
        public Task<IdentityResult> UpdateEmialAsync(string id, string emial);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IdentityResult> DeleteUserfAsync(string id);
        /// <summary>
        /// 通过id重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回元组,成功就返回IdentityResult结果,用户以及密码,不成功就只返回IdentityResult结果</returns>
        public Task<(IdentityResult, string? password)> ResetPwdAsync(string id);

        /// <summary>
        /// 获取用户的角色
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<IList<string>> GetRolesAsync(User user);

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<IdentityResult> CreateRole(string name);
        /// <summary>
        /// 将角色添加到用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task<IdentityResult> AddRoleToUserAsync(User user, string role);

        public Task<Role> FindRoleByNameAsync(string name);
    }
}