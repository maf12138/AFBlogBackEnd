using Dapper;
using MySqlConnector;
using System.Data;//使用这个包


namespace Infrasturacture.Dapper;


//为什么要封装,Dapper中重载的东西比较多
public class DapperHelper
{
    private readonly IDbConnection _dbConnection = new MySqlConnection();

    public DapperHelper()
    {
        _dbConnection.ConnectionString = ConnectionString;
    }
    public string ConnectionString => MyDbContext.ConnectionString;//get


    /// <summary>
    /// Query
    /// </summary>
    /// <typeparam name="T">映射的实体</typeparam>
    /// <param name="sql">SQL语句</param>
    /// <param name="param">参数</param>
    /// <param name="transaction">事务</param>
    /// <param name="commandTimeout">command超时时间(秒)</param>
    /// <param name="commandType">传输的类型,SQL语句or存储过程</param>
    /// <returns></returns>
    public T QueryFirst<T>(
        string sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        return _dbConnection.QueryFirst<T>(sql, param, transaction, commandTimeout, commandType);
    }


    /// <summary>
    /// 
    /// <typeparam name="T">映射的实体</typeparam>
    /// <param name="sql">SQL语句</param>
    /// <param name="param">参数</param>
    /// <param name="transaction">事务</param>
    /// <param name="commandTimeout">command超时时间(秒)</param>
    /// <param name="buffered">是否缓存结果</param>
    /// <param name="commandType">传输的类型,SQL语句or存储过程</param>
    /// <returns></returns>
    public IEnumerable<T> Query<T>(
           string sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null,
        bool buffered = true,
        CommandType? commandType = null
        )
    {
        return _dbConnection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
    }

    /// <summary>
    /// 执行方法
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <param name="transaction"></param>
    /// <param name="commandTimeout"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public int Execute(
               string sql,
                      object param = null,
                             IDbTransaction transaction = null,
                                    int? commandTimeout = null,
                                           CommandType? commandType = null
               )
    {
        return _dbConnection.Execute(sql, param, transaction, commandTimeout, commandType);
    }

}
