using Domain.Interface;

namespace Domain.Entities
{
    public class Article : IHasDeletionTime, IHasCreationTime, ISoftDelete, IHasModificationTime, IAggregateRoot
    {
        public int Id { get; init; }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 文章副标题/写作背景
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 文章缩略图路径,
        /// </summary>
        public string? Thumbnail { get; set; }
        /// <summary>
        /// 文章内容
        /// </summary>
        public string? Content { get; set; }
        //存储物理路径
        public string ?Path { get; set; }
        //配置可空将关系更改为不级联删除
        //https://learn.microsoft.com/zh-cn/ef/core/saving/cascade-delete#:~:text=%E5%B0%9D%E8%AF%95%E5%88%9B%E5%BB%BA%E9%85%8D%E7%BD%AE%E4%BA%86%E8%BF%99%E4%BA%9B%E7%BA%A7%E8%81%94%E7%9A%84%20SQL%20Server%20%E6%95%B0%E6%8D%AE%E5%BA%93%E4%BC%9A%E5%AF%BC%E8%87%B4%E4%BB%A5%E4%B8%8B%E5%BC%82%E5%B8%B8%EF%BC%9A
        /// <summary>
        /// 文章作者
        /// </summary>
        public User? Author { get; private set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int Likes { get; private set; } = 0;
        /// <summary>
        /// 批评数
        /// </summary>
        public int DisLikes { get; private set; } = 0;
        //适度冗余
        /// <summary>
        /// 作者名字
        /// </summary>
        public string AuthorName { get; private set; }

        public DateTime? DeleteTime { get; private set; }

        public DateTime CreateTime { get; init; } = DateTime.Now;

        public bool IsDeleted { get; private set; } = false;

        /// <summary>
        /// 文章标签
        /// </summary>
        public virtual ICollection<Tag>? Tags { get; private set; }
        /// <summary>
        /// 文章分类
        /// </summary>
        public Category? Category { get; private set; }

        public bool IsDraft { get; set; } = false;//是否为草稿,默认不是,是则不显示
        public DateTime? ModificationTime { get; private set; }

        public void SoftDelete()
        {
            this.IsDeleted = true;
            DeleteTime = DateTime.Now;
        }
        //EF Core 无法使用构造函数设置导航属性
        //https://learn.microsoft.com/zh-cn/ef/core/modeling/constructors
        private Article() { }

        //给静态服务器上传用的
        public Article(string title, string? description, string path, User user)
        {
            Title = title;
            Path = path;
            Description = description;
            Author = user;
            AuthorName = user.UserName;
        }
        // 给接的前端用的,还需指定标签和分类
        public Article(string title,string summary,string content,string thumbnail,bool isDraft,User author)
        {
            Title = title;
            Description = summary;
            Content = content;
            Thumbnail = thumbnail;
            IsDraft = isDraft;
            Author = author;
            AuthorName = author.UserName;
        }

        public Article specify(User user)
        {
            Author = user;
            AuthorName = user.UserName;
            return this;
        }

        public Article ChangeTitle(string title)
        {
            this.Title = title;
            return this;
        }


        public Article ChangeType(Category type)
        {
            this.Category = type;
            return this;
        }

        public Article ChangeTags(List<Tag> tags)
        {
            this.Tags = tags;
            return this;
        }
        public int ViewCount { get; private set; } = 1;
        public void LikeIt()
        {
            this.Likes++;
        }
        public void ViewIt()
        {
            this.ViewCount++;
        }

        public void DisLikeIt()
        {
            this.Likes--;
        }
        public void Modified()
        {
            this.ModificationTime = DateTime.Now;
        }

        public Article ChangeSummary(string summary)
        {
            this.Description = summary;
            return this;
        }
    }
}

