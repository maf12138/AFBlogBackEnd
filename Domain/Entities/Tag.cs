namespace Domain.Entities
{
    //好像这个类不用映射到数据库
    public class Tag
    {
        public int Id { get; init; }
        public string TagName { get; set; }

        public virtual ICollection<Article> Articles { get; } = new List<Article>();

        public Tag(string tagName)
        {
            this.TagName = tagName;
        }

        public int GetArticleCount()
        {
            return Articles.Count;//这里需要底层查询的时候include上article
        }
    }
}
