using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface;

public interface IDbConnectionFactory
{
    MySqlConnection CreateConnection();
}
