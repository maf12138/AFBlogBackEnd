namespace BackEndWebAPI.VO;

public class HotArticleVo
{
    public int id { get; set; }
    public string title { get; set; }
    public string thumbnail { get; set; }
    public string createTime { get; set; }
    public long viewCount { get; set; }
}
