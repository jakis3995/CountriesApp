using System.Data.SqlClient;

namespace Countries
{
    interface IDbConnection
    {
        int CreateConnection(string connectionString);
        SqlConnection GetSqlConnection();
    }
}
