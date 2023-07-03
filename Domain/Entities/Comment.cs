using Domain.Interface;

namespace Domain.Entities
{
    public class Comment : IHasCreationTime, IHasDeletionTime, IHasModificationTime, ISoftDelete, IAggregateRoot
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime CreateTime { get; init; } = DateTime.Now;

        public DateTime? DeleteTime { get; set; }

        public DateTime? ModificationTime { get; set; }

        public int Likes { get; set; } = 0;

        public int DisLike { get; set; } = 0;

        public bool IsDeleted { get; set; } = false;

        public void SoftDelete()
        {
            this.IsDeleted = true;
            this.DeleteTime = DateTime.Now;
        }

        public string Content { get; set; }

        public string? UserName { get; private set; }//适度冗余

        public User? CommentUser { get; private set; }

        public int ArticleId { get; set; }


        public Article? Article { get; private set; }

        private Comment() { }
        public Comment(string content, User user, Article article)
        {
            this.Content = content;
            this.Article = article;
            this.CommentUser = user;
            this.ArticleId = article.Id;
            this.UserName = user.UserName;
        }/*
        /// <summary>
        /// 指定文章与评论者,调用之前先参数校验X
        /// 功能迁移到构造函数中,维护数据一致性
        /// </summary>
        /// <param name="user"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        public Comment SpecifyCommentBelong(User user,Article article) //怎么赋值安全呢?用Builder模式可不可以?
        {
            this.Article = article;
            this.CommentUser = user;
            this.ArticleId = article.Id;
            this.UserName = user.UserName;
            return this;
        }*/

        /// <summary>
        /// 应该加个校验,在规定时间内并且没有回复的评论存在
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public Comment ChangeContent(string content)
        {
            this.Content = content;
            this.ModificationTime = DateTime.Now;
            return this;
        }

        public void LikeIt()
        {
            this.Likes++;
        }
        public void DisLikeIt()
        {
            this.DisLike++;
        }

        public void UpdateComment(string content)
        {
            this.Content = content;
        }
    }
}

