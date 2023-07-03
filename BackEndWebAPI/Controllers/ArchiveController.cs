using AutoMapper;
using BackEndWebAPI.VO;
using BackEndWebAPI.WebAPIExtensions;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEndWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArchiveController : ControllerBase
{
    private readonly IArticleRepository _articleRepository;
    private readonly IMapper _mapper;

    public ArchiveController(IArticleRepository articleRepository, IMapper mapper)
    {
        _mapper = mapper;
        _articleRepository = articleRepository;
    }

    // 获取归档列表
    [HttpGet("/api/archive/archiveList")]
    public async Task<ResponseResult<PageVo<ArchiveVo>>> GetArchiveList([FromQuery]int pageNum,int pageSize)
    {
        var articles = await _articleRepository.GetAllArticlesAsync();
        var groupedArticles = articles.GroupBy(a => a.CreateTime.Year).Skip((pageNum-1) * pageSize).Take(pageSize);
        var archiveVoList = new List<ArchiveVo>();
        foreach (var group in groupedArticles)
        {
            var year = group.Key;
            //可以尝试用automapper
            var articleList = group.Select(a => new HotArticleVo
            {
                createTime = a.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                id = a.Id,
                thumbnail = a.Thumbnail,
                title = a.Title,
                viewCount = a.ViewCount
            }).OrderByDescending(a => a.createTime).ToList();
            var archiveVo = new ArchiveVo
            {
                articles = articleList,
                year = year
            };
            archiveVoList.Add(archiveVo);
        }
        var total = articles.Count();
        var pageVo = new PageVo<ArchiveVo>(total, archiveVoList);

        return new ResponseResult<PageVo<ArchiveVo>>(200, "操作成功",pageVo );
    }

    //获取归档及其数量,用于组件
    [HttpGet("/api/archive/archiveCountList")]
    public async Task<ResponseResult<PageVo<ArchiveCountVo>>> GetArchiveCountList([FromQuery]int pageNum, [FromQuery]int pageSize)
    {
        var articles = await _articleRepository.GetAllArticlesAsync();
        var groupedArticles = articles
            //匿名类型
            .GroupBy(a => new { a.CreateTime.Year, a.CreateTime.Month })
            .Skip((pageNum-1) * pageSize)
            .Take(pageSize);

        var archiveCountVoList = groupedArticles.Select(group => new ArchiveCountVo
        {
            date= group.Key.Year.ToString()+"/"+group.Key.Month.ToString(),
            count = group.Count()
        }).ToList();
        var pageVo = new PageVo<ArchiveCountVo>(articles.Count(), archiveCountVoList);
        return new ResponseResult<PageVo<ArchiveCountVo>>(200, "操作成功", pageVo);
    }
}
