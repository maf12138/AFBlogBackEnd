using BackEndWebAPI.Attributes;
using BackEndWebAPI.VO;
using BackEndWebAPI.WebAPIExtensions;
using Dapper;
using Domain;
using Domain.Entities;
using Domain.Interface;
using Infrasturacture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;


namespace BackEndWebAPI.Controllers.CommentController
{

    [Route("api/[controller]")]
    [UnitOfWork(typeof(BlogDbContext))]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ArticleDomainService articleDomainService;
        private readonly CommentDomainService commentDomainService;
        private readonly IdentityDomainService identityDomainService;
        private readonly BlogDbContext blogDbContext;
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public CommentController(ArticleDomainService articleDomainService, IdentityDomainService identityDomainService, CommentDomainService commentDomainService, BlogDbContext blogDbContext, IDbConnectionFactory dbConnectionFactory)
        {
            this.articleDomainService = articleDomainService;
            this.identityDomainService = identityDomainService;
            this.commentDomainService = commentDomainService;
            this.blogDbContext = blogDbContext;
            _dbConnectionFactory = dbConnectionFactory;
        }

        //获取指定文章的评论
        [HttpGet("/api/comment/commentList")]
        public async Task<ResponseResult<PageVo<CommentVO>>> GetCurrentArticleComments([FromQuery] int articleId, [FromQuery] int pageNum, [FromQuery] int pageSize)
        {
            await using MySqlConnection mySqlConnection = _dbConnectionFactory.CreateConnection();
            var comments = await mySqlConnection.QueryAsync<CommentVO>(
                @"SELECT T_Comments.Id,ArticleId,avatar,T_Comments.CreateTime,T_Comments.UserName,T_Comments.Content
                  From T_Comments 
                Left Join T_Users On CommentUserId = T_Users.Id
                Where ArticleId = @articleId"
        , new
        {
            articleId
        });
            //    comments = blogDbContext.Comments.Where(a => a.ArticleId == articleId).Include(a => a.CommentUser).OrderByDescending(a => a.CreateTime);
            int total = comments.Count();
            if (comments == null)
            {
                return new ResponseResult<PageVo<CommentVO>>(200, "没有评论", null);
            }
            comments = comments.Skip((pageNum - 1) * pageSize).Take(pageSize);
            var pageVo = new PageVo<CommentVO>(total, comments.ToList());
            return new ResponseResult<PageVo<CommentVO>>(200, "获取成功", pageVo);
        }

        //个人添加评论
        [HttpPost("/api/comment")]
        [Authorize]
        public async Task<ResponseResult<object>> AddComment([FromHeader] string token, AddCommentRequest request)
        {
            var article = await articleDomainService.FindArticleByIdAsync(request.articleId);
            if (article == null)
            {
                return new ResponseResult<object>(404, "未找到文章", null);
            }
            var user = await identityDomainService.FIndUserByTokenAsync(token);
            if (user == null)
            {
                return new ResponseResult<object>(404, "未找到用户", null);
            }
            //怎么优化这段,使其封装性更好
            var comment = new Comment(request.content, user, article);
            await blogDbContext.AddAsync(comment);
            await blogDbContext.SaveChangesAsync();
            return new ResponseResult<object>(200, "评论成功", "");
        }

        //个人更新评论
        [HttpPut("/api/comment")]
        [Authorize]
        public async Task<ResponseResult<object>> UpdateComment(Guid id, int articleId, string content, [FromHeader] string token)
        {
            //自己的逻辑中articleid没用上,推测可以通过articleId查到文章对应的comments然后再做处理
            var user = await identityDomainService.FIndUserByTokenAsync(token);
            if (user == null)
            {
                return new ResponseResult<object>(404, "未找到用户", null);
            }
            var comment = await commentDomainService.FindCommentByIdAsync(id);
            if (comment == null)
            {
                return new ResponseResult<object>(404, "未找到评论", null);
            }
            if (comment.CommentUser.Id == user.Id)//居然不为空
            {
                comment.UpdateComment(content);
                await blogDbContext.SaveChangesAsync();
                return new ResponseResult<object>(200, "更新成功", null);
            }
            else
            {
                return new ResponseResult<object>(404, "只能更新自己的评论", null);
            }

        }
        //个人删除评论   
        [HttpDelete("/api/comment/{id}")]
        [Authorize]
        public async Task<ResponseResult<object>> DeleteComment(Guid id, [FromHeader] string token)
        {
            var user = await identityDomainService.FIndUserByTokenAsync(token);
            if (user == null)
            {
                return new ResponseResult<object>(404, "未找到用户", null);
            }
            var comment = await commentDomainService.FindCommentByIdAsync(id);
            if (comment == null)
            {
                return new ResponseResult<object>(404, "未找到评论", null);
            }
            if (comment.CommentUser.Id == user.Id)
            {
                blogDbContext.Remove(comment);
                await blogDbContext.SaveChangesAsync();
                return new ResponseResult<object>(200, "删除成功", null);
            }
            else
            {
                return new ResponseResult<object>(404, "只能删除自己的评论", null);
            }

        }
    }
}
