using MySqlConnector;
namespace Domain.Interface;

public interface IDbConnectionFactory
{
    MySqlConnection CreateConnection();
}
