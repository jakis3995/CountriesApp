namespace Countries
{
    interface ICountryInfoGrabber
    {
        int CheckGetAbility(string countryName);
        Country GetCountryInfo(string countryName);
    }
}
