using AutoMapper;
using BackEndWebAPI.VO;
using BackEndWebAPI.WebAPIExtensions;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace BackEndWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;

        public TagController(IArticleRepository articleRepository, IMapper mapper)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
        }

        //获取所有标签和他包含的文章数量
        [HttpGet("/api/tag/tagCountList")]
        public async Task<ResponseResult<IEnumerable<TagCountVO>>> GetTagCountList()
        {
            var tags = (await _articleRepository.GetAllTags()).OrderBy(tag => tag.TagName);
            var tagVos = _mapper.Map<IEnumerable<TagCountVO>>(tags);//延迟查询
            return new ResponseResult<IEnumerable<TagCountVO>>(200, "操作成功", tagVos.ToArray());
        }

        [HttpGet("/api/GetTagArticleCount")]
        public async Task<ActionResult<int>> GetTagArticleCount(int id)
        {
            var tag = await _articleRepository.GetTagByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag.GetArticleCount());
        }
    }
}
