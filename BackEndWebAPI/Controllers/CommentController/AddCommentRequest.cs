namespace BackEndWebAPI.Controllers.CommentController
{
    public record AddCommentRequest(int articleId,string content);
}