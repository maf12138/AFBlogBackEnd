using Domain.Entities;

namespace Domain
{
    /// <summary>
    /// 领域服务需要使用仓储接口来通过持久层读写数据
    /// 增删查不属于业务,直接写在这里
    /// 封装数据访问,(这样更换ORM可能只需少量更改业务层)-- EF Core 更推荐直接使用DbContext,也就是对相关的直接返回一个
    /// _DbContext.Articles 来操作
    /// 开发时的自定义Error会转发给客户端,所以不需要在这里处理异常逻辑
    /// </summary>
    public interface IArticleRepository
    {
        Task<Article?> FindArticleByIdAsync(int id);
        Task<Article?> FindArticleByIdIncludeAsync(int id);//包括文章的相关信息的查询
        Task<IEnumerable<Article>> GetAllArticlesAsync();
        Task<IEnumerable<Article>> GetAllArticlesAndDetailAsync();
        Task AddArticleAsync(Article article);
        Task DeleteArticle(int id);
        Task<IEnumerable<Article>> GetArticlesByTags(string tag);
        Task<IEnumerable<Article>> GetArticlesByCategory(string category);
        Task<IEnumerable<Category>> GetAllCategories();
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Article>> Search(string keyword);
        Task<IEnumerable<Article>> GetTopArticles(int count);//获取热门文章列表（按照阅读量或评论数等指标）。
        Task<IEnumerable<Tag>> GetAllTags();//获取所有Tag

        //根据id获取tag
        Task<Tag?> GetTagByIdAsync(int id);

        Task<IEnumerable<Tag>?> GetArticleTags(int articleId);
    }


}
