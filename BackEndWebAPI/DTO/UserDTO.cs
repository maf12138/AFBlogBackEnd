namespace BackEndWebAPI.DTO;

//有空用FluentValadation改造
public class UserDTO
{
    public string id { get; set; }
    public string nickName { get; set; }
    public string userName { get; set; }
    public string signature { get; set; }
    public string email { get; set; }
    public string phonenumber { get; set; }
    public string sex { get; set; }
    public string avatar { get; set; }
}
