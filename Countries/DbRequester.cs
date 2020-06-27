using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Countries
{
    class DbRequester : IDbRequester
    {
        /* Класс, напрямую взаимодействующий с базой данных и включающий в себя запросы к базе
         * данных
         */
        private string connectionString;
        private SqlConnection sqlConnection;

        public DbRequester(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int CreateConnection()
        {
            IDbConnection dataBaseConnection = new DataBaseConnection();
            int result = dataBaseConnection.CreateConnection(connectionString);
            this.sqlConnection = dataBaseConnection.GetSqlConnection();

            return result;
        }

        public int FindCity(string cityName)
        {
            /* Запрос по поиску города
             */
            int foundId = -1;

            string cityIdSearch = "SELECT id FROM Cities WHERE name = '" + cityName + "'";
            SqlCommand cityIdSearchCommand = new SqlCommand(cityIdSearch, sqlConnection);
            cityIdSearchCommand.Connection.Open();
            using (DbDataReader cityIdReader = cityIdSearchCommand.ExecuteReader())
            {
                // Если есть результаты
                if (cityIdReader.HasRows)
                {
                    //  Заносим в cityId первый результат поиска города
                    cityIdReader.Read();
                    foundId = cityIdReader.GetInt32(cityIdReader.GetOrdinal("id"));
                }
            }
            cityIdSearchCommand.Connection.Close();

            return foundId;
        }

        public int AddCity(string cityName)
        {
            /* Запрос по добавлению города
             */
            int cityId = 0;

            string cityInsert = "INSERT INTO Cities (name) " +
                                "VALUES ('" + cityName + "'); " +
                                "SELECT CAST(scope_identity() AS int)";
            SqlCommand cityInsertCommand = new SqlCommand(cityInsert, sqlConnection);
            cityInsertCommand.Connection.Open();
            cityId = (Int32)cityInsertCommand.ExecuteScalar();
            cityInsertCommand.Connection.Close();

            return cityId;
        }

        public int FindRegion(string regionName)
        {
            /* Запрос по поиску региона
             */
            int foundId = -1;

            string regionIdSearch = "SELECT id FROM Regions " +
                                    "WHERE name = '" + regionName + "'";
            SqlCommand regionIdSearchCommand = new SqlCommand(regionIdSearch, sqlConnection);
            regionIdSearchCommand.Connection.Open();
            using (DbDataReader regionIdReader = regionIdSearchCommand.ExecuteReader())
            {
                // Если есть результаты
                if (regionIdReader.HasRows)
                {
                    // Заносим в regionId первый результат поиска региона
                    regionIdReader.Read();
                    foundId = regionIdReader.GetInt32(regionIdReader.GetOrdinal("id"));
                }
            }
            regionIdSearchCommand.Connection.Close();

            return foundId;
        }

        public int AddRegion(string regionName)
        {
            /* Запрос по добавлению региона
             */
            int regionId = 0;

            string regionInsert = "INSERT INTO Regions (name) " +
                                  "VALUES ('" + regionName + "'); " +
                                  "SELECT CAST(scope_identity() AS int)";
            SqlCommand regionInsertCommand = new SqlCommand(regionInsert,
                sqlConnection);
            regionInsertCommand.Connection.Open();
            regionId = (Int32)regionInsertCommand.ExecuteScalar();
            regionInsertCommand.Connection.Close();

            return regionId;
        }

        public int FindCountry(string countryCode)
        {
            /* Запрос по поиску страны
             */
            int foundId = -1;

            string countryIdSearch = "SELECT id FROM Countries " +
                                     "WHERE code = '" + countryCode + "'";
            SqlCommand countryIdSearchCommand = new SqlCommand(countryIdSearch, sqlConnection);
            countryIdSearchCommand.Connection.Open();

            using (DbDataReader countryIdReader = countryIdSearchCommand.ExecuteReader())
            {
                // Если есть результаты поиска
                if (countryIdReader.HasRows)
                {
                    // Обновляются данные страны по её найденному id
                    countryIdReader.Read();
                    foundId = countryIdReader.GetInt32(countryIdReader.GetOrdinal("id"));
                }
            }
            countryIdSearchCommand.Connection.Close();

            return foundId;
        }

        public void AddCountry(Country country, int cityId, int regionId)
        {
            /* Запрос по добавлению страны
             */
            string countryInsert = "INSERT INTO Countries " +
                                   "(name, code, capital, area, population, region) " +
                                   "VALUES ('" + country.Name + "', '" + country.Code + "', " +
                                   "" + cityId + ", " + country.Area + ", " +
                                   "" + country.Population + ", " + regionId + ")";
            SqlCommand countryInsertCommand = new SqlCommand(countryInsert,
                sqlConnection);
            countryInsertCommand.Connection.Open();
            countryInsertCommand.ExecuteNonQuery();
            countryInsertCommand.Connection.Close();
        }

        public void UpdateCountry(Country country, int cityId, int regionId, int countryId)
        {
            /* Запрос по обновлению страны
             */
            string countryUpdate = "UPDATE Countries SET " +
                                   "name = '" + country.Name + "', capital = " + cityId + ", " +
                                   "area = " + country.Area + ", " +
                                   "population = " + country.Population + ", " +
                                   "region = " + regionId + " WHERE id = " + countryId + "";
            SqlCommand countryUpdateCommand = new SqlCommand(countryUpdate,
                sqlConnection);
            countryUpdateCommand.Connection.Open();
            countryUpdateCommand.ExecuteNonQuery();
            countryUpdateCommand.Connection.Close();
        }

        public DataTable GetCountriesTable()
        {
            /* Запрос по извлечению списка стран, сохранённых в базе данных
             */
            DataTable dataTable = new DataTable();

            string dataQuery = "SELECT c.name AS 'Название', c.code AS 'Код страны', " +
                               "Cities.name AS 'Столица', c.area AS 'Площадь', " +
                               "c.population AS 'Население', Regions.name AS 'Регион' " +
                               "FROM Countries c INNER JOIN Cities on c.capital = Cities.id " +
                               "INNER JOIN Regions on c.region = Regions.id";
            SqlDataAdapter dataQueryCommand = new SqlDataAdapter(dataQuery, sqlConnection);
            dataQueryCommand.Fill(dataTable);

            return dataTable;
        }
    }
}
