

namespace BackEndWebAPI.VO;

public class BlogUserLoginVo
{
    public string token { get; set; }
    //
    public UserInfoVo userInfo { get; set; }

    public BlogUserLoginVo(string token, UserInfoVo userInfoVo)
    {
        this.token = token;
        this.userInfo = userInfoVo;
    }
}
