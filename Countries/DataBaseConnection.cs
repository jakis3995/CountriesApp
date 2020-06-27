using System;
using System.Data.SqlClient;

namespace Countries
{
    class DataBaseConnection
    {
        private SqlConnection sqlConnection;

        public int CreateConnection(string connectionString)
        {
            int result = -1;

            try
            {
                this.sqlConnection = new SqlConnection(connectionString);
                result = 0;
            }
            catch (InvalidOperationException exception)
            {
                result = 1;
            }
            catch (ArgumentException exception)
            {
                result = 2;
            }

            return result;
        }

        public SqlConnection GetSqlConnection()
        {
            return this.sqlConnection;
        }
    }
}
