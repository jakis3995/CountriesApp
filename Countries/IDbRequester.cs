using System.Data;

namespace Countries
{
    interface IDbRequester
    {
        int CreateConnection();
        int FindCity(string cityName);
        int AddCity(string cityName);
        int FindRegion(string regionName);
        int AddRegion(string regionName);
        int FindCountry(string countryCode);
        void AddCountry(Country country, int cityId, int regionId);
        void UpdateCountry(Country country, int cityId, int regionId, int countryId);
        DataTable GetCountriesTable();
    }
}
