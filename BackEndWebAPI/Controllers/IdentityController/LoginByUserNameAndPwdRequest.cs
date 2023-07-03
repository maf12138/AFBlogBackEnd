namespace BackEndWebAPI.Controllers.IdentityController
{
    public partial class LoginController
    {
        public record LoginByUserNameAndPwdRequest(string userName,string password);


    }
}
