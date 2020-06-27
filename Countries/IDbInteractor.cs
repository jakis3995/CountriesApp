namespace Countries
{
    interface IDbInteractor
    {
        int CreateOrUpdateCountry(Country country);
    }
}
