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
            //DataBaseConnection dbConnection = new DataBaseConnection();
            DbRequester dbRequester = new DbRequester(connectionString);
            int connectionErrorCode = dbRequester.CreateConnection();
            if (connectionErrorCode == 0)
            {
                cityId = dbRequester.FindCity(country.capital);
                if (cityId == -1)
                {
                    cityId = dbRequester.AddCity(country.name);
                }

                regionId = dbRequester.FindRegion(country.region);
                if (regionId == -1)
                {
                    regionId = dbRequester.AddRegion(country.region);
                }

                countryId = dbRequester.FindCountry(country.code);
                if (countryId == -1)
                {
                    dbRequester.AddCountry(country, cityId, regionId);
                    resultCode = 1;
                }
                else
                {
                    dbRequester.UpdateCountry(country, cityId, regionId, countryId);
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
