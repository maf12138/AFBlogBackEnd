using Domain.Interface;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser<Guid>, IHasCreationTime, IHasDeletionTime, ISoftDelete, IAggregateRoot
    {
        //public Guid UserId { get; init; }
        /// <summary>
        /// 头像url
        /// </summary>
        public string? avatar { get; set; }
        /// <summary>
        /// 昵称,可不设置
        /// </summary>
        public string? NickName { get; set; }
        /// <summary>
        /// 0男,1女,2未知
        /// </summary>
        public string? sex { get; set; }

        public string? signature { get; set; }
        public DateTime CreateTime { get; init; } = DateTime.Now;

        public DateTime? DeleteTime { get; set; }

        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 公有构造函数供EFCore以及开发人员使用,不使用私有构造函数
        /// </summary>
        /// <param name="UserName"></param>
        /// 
        public User(string UserName)//identity密码在上层传入
        {
            this.UserName = UserName;
        }

        public void SoftDelete()
        {
            this.IsDeleted = true;
            DeleteTime = DateTime.Now;
        }
        public long JWTVersion { get; set; }   
        
    }
}
