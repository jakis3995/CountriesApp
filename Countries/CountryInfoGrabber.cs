using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Countries
{
    class CountryInfoGrabber
    {
        private string url;

        public CountryInfoGrabber(string url)
        {
            this.url = url;
        }

        public int CheckGetAbility(string countryName)
        {
            int code = -1;

            try
            {
                WebClient webclient = new WebClient();
                webclient.DownloadString(url + countryName);
                code = 0;
            }
            catch (WebException exception)
            {
                var response = (HttpWebResponse) exception.Response;
                // Если не удаётся найти страну
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    code = 1;
                }
                else
                {
                    code = 2;
                }
            }

            return code;
        }

        public Country GetCountryInfo(string countryName)
        {
            WebClient webclient = new WebClient();
            /*
             *    Производит попытку подключения к внешней странице для извлечения инофрмации
             * по искомой стране
             */

            /*
             *    Складывается URL-адрес страницы из основного URL (string url) и 
             * введённого названия страны на английском языке.
             */
            string data = webclient.DownloadString(url + countryName);
            var jsonObject = JsonConvert.DeserializeObject<JArray>(data)
                .ToObject<List<JObject>>().FirstOrDefault();
            string name, code, capital, region;
            float area;
            long population;
            name = jsonObject["name"].ToString();
            code = jsonObject["alpha2Code"].ToString();
            capital = jsonObject["capital"].ToString();
            region = jsonObject["region"].ToString();
            area = float.Parse(jsonObject["area"].ToString());
            population = int.Parse(jsonObject["population"].ToString());
            // Создаётся экземпляр найденной страны
            Country country = new Country(name, code, capital, area, population, region);

            return country;
        }
    }
}
