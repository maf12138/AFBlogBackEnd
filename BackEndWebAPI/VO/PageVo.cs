namespace BackEndWebAPI.VO
{
    //用于便于分页,有点泛型编程的意思
    public class PageVo<T> where T : class
    {
        //总计条数
        public long total { get; set; }
        //数据
        public List<T>? Rows { get; set; }

        public PageVo(long total, List<T>? rows)
        {
            this.total = total;
            Rows = rows;
        }

    }
}
