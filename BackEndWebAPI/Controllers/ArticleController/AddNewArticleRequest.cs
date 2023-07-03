namespace BackEndWebAPI.Controllers.ArticleController
{
    public record AddNewArticleRequest(string Title, string Path, string Describe);
}