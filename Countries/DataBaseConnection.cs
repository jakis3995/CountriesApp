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
                sqlConnection.Open();
                sqlConnection.Close();
                result = 0;
            }
            catch (InvalidOperationException)
            {
                result = 1;
            }
            catch (ArgumentException)
            {
                result = 2;
            }
            catch (SqlException)
            {
                result = 3;
            }

            return result;
        }

        public SqlConnection GetSqlConnection()
        {
            return this.sqlConnection;
        }
    }
}
