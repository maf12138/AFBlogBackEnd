using Domain.Interface;
using MySqlConnector;

namespace Infrasturacture.Dapper;

public class DapperDbConnectionFactory : IDbConnectionFactory
{
    public MySqlConnection CreateConnection()
    {
        var conn = Environment.GetEnvironmentVariable("MAF_MYSQL_CONN");
        return new MySqlConnection(conn);
    }
}
