namespace BackEndWebAPI.DTO
{
    public class ArticleNewDTO
    {
        public string title { get; set; }

        public string category { get; set; }

        public string content { get; set; }

        public string summary { get; set; }

        /**
         * 标签名列表
         */
        public List<string> tags { get; set; }

        public string thumbnail { get; set; }

        public bool isDraft { get; set; }
    }
}
