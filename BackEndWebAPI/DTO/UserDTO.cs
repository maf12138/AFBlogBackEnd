using FluentValidation;
using Infrasturacture;

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

/// <summary>
/// 用户修改信息校验类
/// </summary>
public class UserDTOValidator : AbstractValidator<UserDTO> 
{
    private readonly BlogDbContext _blogDbContext;
    public UserDTOValidator(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
        RuleFor(x=>x.email)
            .NotNull().WithMessage("邮箱不能为空")
            .EmailAddress().WithMessage("邮箱格式不正确");
        RuleFor(x=>x.nickName)
            .MaximumLength(10)
            .WithMessage("昵称长度不能超过10");
        RuleFor(x => x.userName)
            .NotNull().WithMessage("用户名不能为空")
            .Length(1, 10).WithMessage("用户名长度在1-10之间")
            .Must(name=> BeUniqueUserName(name)).WithMessage("用户名已存在");
        RuleFor(x=>x.phonenumber)
            .Matches(@"^\d{11}$").WithMessage("手机号格式不正确,必须为11位");
        RuleFor(x=>x.sex)
            .Must(x=>x=="1"||x=="0").WithMessage("性别只能为男或女");
    }

    private bool BeUniqueUserName(string userName)
    {
        return !_blogDbContext.Users.Any(u => u.UserName == userName);
    }
}

