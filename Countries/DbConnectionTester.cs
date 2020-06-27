using System;
using System.Data.SqlClient;
using System.Text;

namespace Countries
{
    class DbConnectionTester : IDbConnectionTester
    {
        /* Класс, проверяющий возможность подключиться к базе данных по информации, введённой
         * пользователем
         */
        public string ConnectionStringBuilder(string dataSource, string databaseName,
            bool authInfo, string userId, string password)
        {
            string connectionString;

            StringBuilder connectionStringBuilder = new StringBuilder();
            connectionStringBuilder.Append("Data Source=" + dataSource + ";");
            connectionStringBuilder.Append("Initial Catalog=" + databaseName + ";");
            if (authInfo)
            {
                connectionStringBuilder.Append("User Id=" + userId + ";");
                connectionStringBuilder.Append("Password=" + password);
            }
            else
            {
                connectionStringBuilder.Append("Integrated Security=True");
            }
            connectionString = connectionStringBuilder.ToString();

            return connectionString;
        }

        public bool IsConnectionTestSuccessful(string connectionString)
        {
            /*
             *    Метод проверяет может ли приложение подключится к базе данных по введённым
             * в текстовые поля данным конфигурации подключения. Возвращает логическое значение:
             * true - если подключение удалось.
             */
            bool success = false;

            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                sqlConnection.Close();

                success = true;
            }
            catch (Exception)
            {
                // ignored
            }

            return success;
        }
    }
}
