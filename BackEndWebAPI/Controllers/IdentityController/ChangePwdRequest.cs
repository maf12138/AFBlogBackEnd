namespace BackEndWebAPI.Controllers.IdentityController
{
    public partial class LoginController
    {
        public record ChangePwdRequest(string Uid, string Pwd, string NewPwd);
    }
}
