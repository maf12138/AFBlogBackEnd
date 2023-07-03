namespace BackEndWebAPI.WebAPIExtensions
{
    public enum AppHttpCodeEnum
    {
        SUCCESS = 200,
        NEED_LOGIN = 401,
        NO_OPERATOR_AUTH = 403,
        RESOURCE_NOT_EXIST = 404,
        SYSTEM_ERROR = 500,
        USERNAME_EXIST = 501,
        PHONENUMBER_EXIST = 502,
        EMAIL_EXIST = 503,
        LOGIN_ERROR = 504,
        REQUIRE_USER_INFO = 505,
        PARAM_NOT_VALID = 506,
        DATE_NOT_VALID = 507
    }

    public static class AppHttpCodeEnumExt
    {
        //usage
        //msg=code.GetMsg()
        public static string GetMsg(this AppHttpCodeEnum code)//enum也能扩展
        {
            switch (code)
            {
                case AppHttpCodeEnum.SUCCESS:
                    return "操作成功";
                case AppHttpCodeEnum.NEED_LOGIN:
                    return "需要登录后操作";
                case AppHttpCodeEnum.NO_OPERATOR_AUTH:
                    return "无权限操作";
                case AppHttpCodeEnum.RESOURCE_NOT_EXIST:
                    return "请求的资源不存在";
                case AppHttpCodeEnum.SYSTEM_ERROR:
                    return "出现错误";
                case AppHttpCodeEnum.USERNAME_EXIST:
                    return "用户名已存在";
                case AppHttpCodeEnum.PHONENUMBER_EXIST:
                    return "手机号已存在";
                case AppHttpCodeEnum.EMAIL_EXIST:
                    return "邮箱已存在";
                case AppHttpCodeEnum.LOGIN_ERROR:
                    return "用户名或密码错误";
                case AppHttpCodeEnum.REQUIRE_USER_INFO:
                    return "用户信息不能为空";
                case AppHttpCodeEnum.PARAM_NOT_VALID:
                    return "请求参数非法";
                case AppHttpCodeEnum.DATE_NOT_VALID:
                    return "日期格式非法";
                default:
                    return "";
            }
        }
    }
}