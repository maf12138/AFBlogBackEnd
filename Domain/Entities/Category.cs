namespace Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

     //   public virtual ICollection<Article> Articles { get; } = new List<Article>();

        // 父分类id,不存在则为-1,此处未使用导航属性,也未显示配置外键
       public int Pid { get;set;}=-1;

    //    public Category ParentCategory { get; set; }
        public Category(string CategoryName)
        {
            this.CategoryName = CategoryName;
        }
    }
}
