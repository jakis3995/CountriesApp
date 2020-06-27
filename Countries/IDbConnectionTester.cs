namespace Countries
{
    interface IDbConnectionTester
    {
        string ConnectionStringBuilder(string dataSource, string databaseName,
            bool authInfo, string userId, string password);

        bool IsConnectionTestSuccessful(string connectionString);
    }
}
