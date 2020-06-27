using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Countries
{
    class DbInteractor
    {
        private string connectionString;

        public DbInteractor(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int CreateOrUpdateCountry(Country country)
        {
            int resultCode;

            int cityId = 0, regionId = 0, countryId = 0;
            DataBaseConnection dbConnection = new DataBaseConnection();
            int connectionErrorCode = dbConnection.CreateConnection(connectionString);
            if (connectionErrorCode == 0)
            {
                cityId = dbConnection.FindCity(country.capital);
                if (cityId == -1)
                {
                    cityId = dbConnection.AddCity(country.name);
                }

                regionId = dbConnection.FindRegion(country.region);
                if (regionId == -1)
                {
                    regionId = dbConnection.AddRegion(country.region);
                }

                countryId = dbConnection.FindCountry(country.code);
                if (countryId == -1)
                {
                    dbConnection.AddCountry(country, cityId, regionId);
                    resultCode = 1;
                }
                else
                {
                    dbConnection.UpdateCountry(country, cityId, regionId, countryId);
                    resultCode = 2;
                }
            }
            else
            {
                resultCode = -1;
            }

            return resultCode;
        }
    }
}
