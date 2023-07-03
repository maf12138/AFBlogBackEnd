using System.ComponentModel.DataAnnotations;

namespace BackEndWebAPI.DTO;

public class ArticleQueryDTO
{
    public int pageNum { get; set; } = 1;

    [Range(1, 40, ErrorMessage = "每页条目数只能在 1-40 之间")]
    public int pageSize { get; set; } = 10;

    public int categoryId { get; set; }

    public int tagId { get; set; }

    [RegularExpression(@"^((19)|(2\d))\d{2}/((0?[1-9])|1[012])$", ErrorMessage = "日期格式错误")]
    public string date { get; set; }
}
