using System.ComponentModel.DataAnnotations;

namespace BackEndWebAPI.DTO;

public class LoginUserDTO
{
    [Required(ErrorMessage = "用户名不能为空")]
    public string userName { get; set; }

    [Required(ErrorMessage = "密码不能为空")]
    public string password { get; set; }
}
