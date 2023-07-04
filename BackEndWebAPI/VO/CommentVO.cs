namespace BackEndWebAPI.VO
{
    public class CommentVO
    {
        public Guid Id { get; set;}

        public int articleId { get; set; }
        public string avatar { get; set; }
       // private Guid createBy { get; set; }//getUserInfo时赋值
        public string createTime { get; set; }
        public string userName { get; set; }
        public string content { get; set; }

       // public bool isAdmin { get; set; }//getUserInfo时赋值
    }
}
