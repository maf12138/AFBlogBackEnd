using Domain.Entities;

namespace Domain
{
    /// <summary>
    ///  直接调用ArticleRepository的方法即可
    /// </summary>
    public class ArticleDomainService
    {
        private readonly IArticleRepository _articleRepository;
        public ArticleDomainService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }
        public async Task<IEnumerable<Article>> GetAllArticles()
        {
            return await _articleRepository.GetAllArticlesAsync();
        }

        public async Task<Article?> FindArticleByIdAsync(int id)
        {
            return await _articleRepository.FindArticleByIdAsync(id);
        }

        public async Task AddArticleAsync(Article article)
        {
            await _articleRepository.AddArticleAsync(article);
        }

        public async Task DeleteArticle(int id)
        {
            await _articleRepository.DeleteArticle(id);
        }

        public async Task<IEnumerable<Article>> Search(string keyword)
        {
            return await _articleRepository.Search(keyword);
        }

        public async Task<IEnumerable<Article>> GetTopArticles(int count)
        {
            return await _articleRepository.GetTopArticles(count);
        }

        public async Task<IEnumerable<Article>> GetArticlesByTags(string tag)
        {
            return await _articleRepository.GetArticlesByTags(tag);
        }

        public async Task<IEnumerable<Article>> GetArticlesByCategory(string category)
        {
            return await _articleRepository.GetArticlesByCategory(category);
        }


    }

}
