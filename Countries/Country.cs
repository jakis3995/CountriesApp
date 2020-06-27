namespace Countries
{
    public class Country
    {
        /*
         * Класс (модель) страны, где присутствуют все нужные по заданию свойства: наименование,
         * код, столица (название), площадь, численность населения и регион (название).
         */
        public string Name
        {
            get;
            set;
        }
        public string Code
        {
            get;
            set;
        }
        public string Capital
        {
            get;
            set;
        }
        public float Area
        {
            get;
            set;
        }
        public long Population
        {
            get;
            set;
        }
        public string Region
        {
            get;
            set;
        }

        public Country(string name, string code, string capital, float area, 
            long population, string region)
        {
            this.Name = name;
            this.Code = code;
            this.Capital = capital;
            this.Area = area;
            this.Population = population;
            this.Region = region;
        }
    }
}
