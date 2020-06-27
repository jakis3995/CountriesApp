namespace Countries
{
    class DbInteractor : IDbInteractor
    {
        /* Класс взаимодействующий с базой данных (на высшем уровне, через DbRequester)
         */
        private string connectionString;

        public DbInteractor(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int CreateOrUpdateCountry(Country country)
        {
            /* Содержит алгоритм добавления страны: сначала добавляется столица в таблицу городов,
             * если её нет, затем добавляется регион в таблицу регионов, если его нет, затем,
             * если страна есть, по ней обновляется информация, если нет - создаётся новая. Работа
             * с запросами производится через экземпляр класса DbRequester
             */
            int resultCode;

            int cityId = 0, regionId = 0, countryId = 0;
            IDbRequester dbRequester = new DbRequester(connectionString);
            int connectionErrorCode = dbRequester.CreateConnection();
            if (connectionErrorCode == 0)
            {
                cityId = dbRequester.FindCity(country.Capital);
                if (cityId == -1)
                {
                    cityId = dbRequester.AddCity(country.Capital);
                }

                regionId = dbRequester.FindRegion(country.Region);
                if (regionId == -1)
                {
                    regionId = dbRequester.AddRegion(country.Region);
                }

                countryId = dbRequester.FindCountry(country.Code);
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
