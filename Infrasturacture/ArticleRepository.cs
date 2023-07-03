using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrasturacture
{
    /// <summary>
    /// 文章仓储,实现了IArticleRepository接口
    /// 保证实体完整性
    /// 因为到底什么时候保存工作单元中的修改是由应用服务层来决定的，仓储和领域层中都不能执行SaveChangesAsync操作。
    /// </summary>
    public class ArticleRepository : IArticleRepository
    {
        private readonly BlogDbContext _blogDbContext;

        public ArticleRepository(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task AddArticleAsync(Article article)
        {
            await _blogDbContext.Articles.AddAsync(article);
        }

        public async Task DeleteArticle(int id)
        {
            var article = await FindArticleByIdAsync(id);
            if (article != null)
            {
                var result = _blogDbContext.Articles.Remove(article);
                await _blogDbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("未找到文章");
            }

        }

        public async Task<Article?> FindArticleByIdAsync(int id)
        {
            return await _blogDbContext.Articles.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Article?> FindArticleByIdIncludeAsync(int id)
        {
            return await _blogDbContext.Articles
                .Include(a => a.Tags)
                .Include(a => a.Category).SingleOrDefaultAsync(article => article.Id == id);
        }

        public async Task<IEnumerable<Article>> GetAllArticlesAsync()
        {
            return await _blogDbContext.Articles.ToListAsync();
        }
        public async Task<IEnumerable<Article>> GetAllArticlesAndDetailAsync()
        {
            return await _blogDbContext.Articles
                .Include(a => a.Tags)
                .Include(a => a.Category).ToListAsync();//立即查询了,如何优化?
        }

        public Task<IEnumerable<Category>> GetAllCategories()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _blogDbContext.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Tag>> GetAllTags()
        {
            return await _blogDbContext.Tags.Include(t => t.Articles).ToListAsync();
        }

        public Task<IEnumerable<Article>> GetArticlesByCategory(string category)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Article>> GetArticlesByTags(string tag)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Tag>?> GetArticleTags(int articleId)
        {
            var article = await _blogDbContext.Articles.Include(a => a.Tags).SingleOrDefaultAsync(a => a.Id == articleId);
            return article.Tags;
        }

        public async Task<Tag?> GetTagByIdAsync(int id)
        {
            return await _blogDbContext.Tags.Include(t => t.Articles).SingleOrDefaultAsync(t => t.Id == id);
        }

        public Task<IEnumerable<Article>> GetTopArticles(int count)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Article>> Search(string keyword)
        {
            throw new NotImplementedException();
        }
    }
}
