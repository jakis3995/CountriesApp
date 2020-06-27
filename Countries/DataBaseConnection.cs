using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Countries
{
    class DataBaseConnection
    {
        SqlConnection sqlConnection;

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

        public int ConnectionOpenCode()
        {
            int result = -1;

            try
            {
                sqlConnection.Open();
                result = 0;
            }
            catch (InvalidOperationException exception)
            {
                result = 1;
            }
            catch (SqlException exception)
            {
                result = 2;
            }

            return result;
        }

        public int OpenConnection()
        {
            int connectionOpenCode = -1;

            if (sqlConnection.State == ConnectionState.Closed)
            {
                connectionOpenCode = this.ConnectionOpenCode();

                switch (connectionOpenCode)
                {
                    case -1:
                        MessageBox.Show("Неизвестная ошибка при подключении к базе данных",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    case 1:
                    case 2:
                        MessageBox.Show("Невозможно подключиться в базе данных. " +
                                        "Уточните конфигурации подключения к базе данных в настройках",
                            "Внимание",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        break;
                }
            }

            return connectionOpenCode;
        }

        public void CloseConnection()
        {
            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public int FindCity(string cityName)
        {
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
            string countryInsert = "INSERT INTO Countries " +
                                   "(name, code, capital, area, population, region) " +
                                   "VALUES ('" + country.name + "', '" + country.code + "', " +
                                   "" + cityId + ", " + country.area + ", " +
                                   "" + country.population + ", " + regionId + ")";
            SqlCommand countryInsertCommand = new SqlCommand(countryInsert,
                sqlConnection);
            countryInsertCommand.Connection.Open();
            countryInsertCommand.ExecuteNonQuery();
            countryInsertCommand.Connection.Close();
        }

        public void UpdateCountry(Country country, int cityId, int regionId, int countryId)
        {
            string countryUpdate = "UPDATE Countries SET " +
                                   "name = '" + country.name + "', capital = " + cityId + ", " +
                                   "area = " + country.area + ", " +
                                   "population = " + country.population + ", " +
                                   "region = " + regionId + " WHERE id = " + countryId + "";
            SqlCommand countryUpdateCommand = new SqlCommand(countryUpdate,
                sqlConnection);
            countryUpdateCommand.Connection.Open();
            countryUpdateCommand.ExecuteNonQuery();
            countryUpdateCommand.Connection.Close();
        }

        public DataTable GetCountriesTable()
        {
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
