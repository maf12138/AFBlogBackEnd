namespace BackEndWebAPI.VO
{
    /// <summary>
    /// 用户信息视图
    /// </summary>
    public class UserInfoVo
    {
        public Guid id { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string nickName { get; set; }
        public string avatar { get; set; }
        public string sex { get; set; }
        public bool isAdmin { get; set; }
        public string phoneNumber { get; set; }

        public string signature { get;set; }


    }
}
