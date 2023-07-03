using Domain.Entities;

namespace Domain
{
    /// <summary>
    /// 领域服务不该是单纯对仓储层的转发，而是对领域对象的操作
    /// </summary>
    public class CommentDomainService
    {
        private readonly ICommentRepository _commentRepository;
        public CommentDomainService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<IEnumerable<Comment>> GetAllComments()
        {
            return await _commentRepository.GetAllComments();
        }
        public async Task<Comment?> FindCommentByIdAsync(Guid id)
        {
            return await _commentRepository.GetCommentById(id);
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await _commentRepository.AddComment(comment);
        }

        public async Task DeleteCommentAsync(Guid id)
        {
            await _commentRepository.DeleteCommentById(id);
        }
        public async Task<IEnumerable<Comment>> GetCommentsByArticleIdAsync(int articleId)
        {
            var comments = await _commentRepository.GetAllComments();
            return comments.Where(c => c.ArticleId == articleId);
        }
        public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(Guid userId)
        {
            var comments = await _commentRepository.GetAllComments();
            return comments.Where(c => c.CommentUser!.Id == userId);
        }
        public async Task<IEnumerable<Comment>> GetCommentsByUserNameAsync(string userName)
        {
            var comments = await _commentRepository.GetAllComments();
            return comments.Where(c => c.CommentUser!.UserName == userName);
        }
    }
}

