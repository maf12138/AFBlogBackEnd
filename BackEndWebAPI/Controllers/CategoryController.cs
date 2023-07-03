using BackEndWebAPI.VO;
using BackEndWebAPI.WebAPIExtensions;
using Domain;
using Domain.Entities;
using Infrasturacture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEndWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly BlogDbContext _blogDbContext;
        
        public CategoryController(IArticleRepository articleRepository,BlogDbContext blogDbContext)
        {
            _articleRepository = articleRepository;
            _blogDbContext = blogDbContext;
        }

        [HttpGet("/api/category/categoryCountList")]
        public async Task<ResponseResult<IEnumerable<CategoryVO>>> GetCategoryCountList()
        {
            var categories = (await _articleRepository.GetAllCategoriesAsync()).OrderBy(category => category.Id);
            var categoryVos = categories.Select(category => new CategoryVO
                {
                    Id = category.Id.ToString(),
                    name = category.CategoryName,
                    count = _blogDbContext.Articles.Count(article => article.Category.Id == category.Id),
                    pid = category.Pid.ToString()
                }); 
            return new ResponseResult<IEnumerable<CategoryVO>>(200, "操作成功", categoryVos.ToArray());
        }
    }
}
