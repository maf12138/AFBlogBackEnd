using Domain.Entities;

namespace Domain
{
    //评论增删查(数据访问)
    public interface ICommentRepository
    {
        /// <summary>
        /// 通过id查找评论
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Comment> GetCommentById(Guid id);
        /// <summary>
        /// 通过用户名查找评论
        /// 可以做下用户名匹配
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<IEnumerable<Comment>> GetCommentsByUserName(string name);
        /// <summary>
        /// 通过用户id查找评论
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IEnumerable<Comment>> GetCommentsByUserId(Guid id);
        /// <summary>
        /// 获取所有评论...,似乎暂时用不上
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Comment>> GetAllComments();
        /// <summary>
        /// 通过id删除评论
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteCommentById(Guid id);
        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public Task AddComment(Comment comment);
        /// <summary>
        /// 更新评论
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public Task UpDateComment(Guid commentId);


    }
}
