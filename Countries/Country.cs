namespace Countries
{
    public class Country
    {
        /*
         * Это класс страны, где присутствуют все нужные по заданию свойства: наименование,
         * код, столица (название), площадь, численность населения и регион (название).
         */
        public string name
        {
            get;
            set;
        }
        public string code
        {
            get;
            set;
        }
        public string capital
        {
            get;
            set;
        }
        public float area
        {
            get;
            set;
        }
        public long population
        {
            get;
            set;
        }
        public string region
        {
            get;
            set;
        }

        public Country(string name, string code, string capital, float area, 
            long population, string region)
        {
            this.name = name;
            this.code = code;
            this.capital = capital;
            this.area = area;
            this.population = population;
            this.region = region;
        }
    }
}
