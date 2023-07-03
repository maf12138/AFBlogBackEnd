using System.ComponentModel.DataAnnotations;

namespace BackEndWebAPI.DTO;

public class ArticleEditDTO
{
   public int Id { get; set; }

    public string title { get; set; }

    public string category { get; set; }

    public string content { get; set; }

    public string summary { get; set; }

    public List<string> tags { get; set; }

    public string? thumbnail { get; set; }

    public bool isDraft { get; set; }
}
