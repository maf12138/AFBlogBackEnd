using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BackEndWebAPI.VO;

public class ArticleListVo
{
    public int id { get; set; }
    //标题
    public string title { get; set; }
    //文章摘要
    public string summary { get; set; }
    //所属分类名
    public string categoryName { get; set; }
    //缩略图
    public string thumbnail { get; set; }
    //访问量
    public int viewCount { get; set; }

    public string createTime { get; set; }
}
