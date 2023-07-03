namespace BackEndWebAPI.Controllers.IdentityController
{
    public record RegisterRequest(string userName, string nickName, string email, string password);

}
