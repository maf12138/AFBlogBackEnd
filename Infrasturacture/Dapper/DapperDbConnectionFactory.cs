using Domain.Interface;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrasturacture.Dapper
{
    public class DapperDbConnectionFactory : IDbConnectionFactory
    {
        public MySqlConnection CreateConnection()
        {
            var conn = Environment.GetEnvironmentVariable("MAF_MYSQL_CONN");
            return new MySqlConnection(conn);
        }
    }
}
