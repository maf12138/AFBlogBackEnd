using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrasturacture
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BlogDbContext>
    {
        public BlogDbContext CreateDbContext(string[] args)
        {
            var optionBuilder = new DbContextOptionsBuilder<BlogDbContext>();
            var connStr = Environment.GetEnvironmentVariable("MAF_MYSQL_CONN");//从电脑环境变量中读取连接字符串
          // string connStr = "host=.;Port=3306;uid=root;pwd=.;Database=Afblog";
            //根据mysql版本配置
            optionBuilder.UseMySql(connStr, new MySqlServerVersion(new Version(8, 0, 32)));
            return new BlogDbContext(optionBuilder.Options);
        }
    }
}
