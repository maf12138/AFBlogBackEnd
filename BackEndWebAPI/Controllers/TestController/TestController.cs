using Domain.Entities;
using Infrasturacture;
using Microsoft.AspNetCore.Mvc;

namespace BackEndWebAPI.Controllers.TestController;


[Route("api/[controller]")]
public class TestController :ControllerBase
{
    private readonly BlogDbContext _blogDbcontext;

    public TestController(BlogDbContext blogDbcontext)
    {
        _blogDbcontext = blogDbcontext;
    }

    [HttpGet]
    public async Task<IActionResult> TransactionScopeFilterTest()
    {
       await  _blogDbcontext.Tags.AddAsync(new Tag("Test_Transaction"));
        await _blogDbcontext.SaveChangesAsync();
        await _blogDbcontext.Tags.AddAsync(new Tag("Test_Transaction2"));
        throw new Exception("Test_Transaction2failed");
       // await     _blogDbcontext.SaveChangesAsync();
       // return Ok();
    }
}
