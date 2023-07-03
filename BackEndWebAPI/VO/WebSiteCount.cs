namespace BackEndWebAPI.VO
{
    public class WebSiteCount
    {
        public int article { get; set; }
        public int category { get; set; }
        public int tag { get; set; }

        public WebSiteCount(int article, int category, int tag)
        {
            this.article = article;
            this.category = category;
            this.tag = tag;
        }
    }
}
