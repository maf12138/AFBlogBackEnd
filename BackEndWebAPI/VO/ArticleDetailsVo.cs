using BackEndWebAPI.DTO;
using Domain.Entities;

namespace BackEndWebAPI.VO;
//DTO 代表服务层需要接收的数据和返回的数据，而 VO 代表展示层需要显示的数据。
public class ArticleDetailsVo
{
    public string Id { get; set; }
    public string content { get; set; }
  //public string title { get; set; }
    public string Summary { get; set; }
    public string Title { get; set; }
    public int categoryId{ get; set; }

    public string categoryName { get; set; }
  //  public Category category { get; set; }// 只有含有自己id和name
    public string createTime { get; set; }

    public ICollection<TagDTO> tags { get; set; }//只有含有自己id和name

    public string thumbnail { get; set; }  // 缩略图路径

    public int viewCount { get; set; }
}
