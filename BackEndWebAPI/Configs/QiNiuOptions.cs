namespace BackEndWebAPI.Configs
{
    //声明为record便于查看是否能获取到配置
    public record QiNiuOptions
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}
