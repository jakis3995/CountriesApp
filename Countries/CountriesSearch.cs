using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data.Common;

namespace Countries
{
    public partial class CountriesSearch : Form
    {
        private Form1 form1;
        string connectionString;
        Country country;
        public CountriesSearch(Form1 form1)
        {
            this.form1 = form1;
            InitializeComponent();
            textBox1.Select();
            connectionString = form1.connectionString;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
             * Обработка нажатия на кнопку "Назад к меню"
             */
            Visible = false;
            form1.Visible = true;
        }

        private void CountriesSearch_FormClosed(object sender, FormClosedEventArgs e)
        {
            form1.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*
             * Обработка нажатия на кнопку "Найти"
             */
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            // Убирает пробелы в введённом названии страны для поиска
            string countryName = textBox1.Text.Replace(" ", "");
            string url = "https://restcountries.eu/rest/v2/name/";
            if (countryName != "")
            {
                WebClient webclient = new WebClient();
                /*
                 *    Производит попытку подключения к внешней странице для извлечения инофрмации
                 * по искомой стране
                 */
                try {
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
                    country = new Country(name, code, capital, area, population, region);

                    textBox2.Text = country.code;
                    textBox3.Text = country.capital;
                    textBox4.Text = country.area.ToString();
                    textBox5.Text = country.population.ToString();
                    textBox6.Text = country.region;
                    textBox7.Text = country.name;
                }
                catch (WebException exception)
                {
                    var response = (HttpWebResponse) exception.Response;
                    // Если не удаётся найти страну
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        MessageBox.Show(
                        "Введено неверное название страны. Пожалуйста, перепроверьте введённое " +
                        "название страны. Обратите внимание, название страны должно быть указано " +
                        "на английском языке",
                        "Сообщение",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                    }
                    // Если не удаётся подключиться
                    else
                    {
                        MessageBox.Show(
                        "Проблемы с Интернет-соединением. Проверьте подключение",
                        "Сообщение",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                    }
                }
                // Если на странице присутствуют некорректные данные
                catch (FormatException formatException)
                {
                    MessageBox.Show(
                    "Проблемы с прочтением данных из внешней базы стран. Введите иную страну",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            else
            {
                MessageBox.Show(
                    "Введите название страны в текстовое поле слева кнопки \"Найти\"", 
                    "Сообщение", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information, 
                    MessageBoxDefaultButton.Button1, 
                    MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Обработка нажатия на клавишу Enter при вводе страны
            if (e.KeyChar == (char)13)
            {
                button2_Click(sender, e);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*
             *    Обработка нажатия на кнопку "Сохранить информацию в базе данных"
             *    Извлекает данные из свойств созданного при поиске экземпляра страны и сохраняет
             * эти данные в базу данных по приведённому в задании алгоритму.
             */
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" ||
                textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "")
            {
                MessageBox.Show(
                    "Нет информации для внесения в базу данных. Вам необходимо найти нужную страну",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                int cityId = 0, regionId = 0, countryId = 0;

                try
                {
                    // Пробует подключиться к базе данных по connectionString
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        // Ищет в базе данных город, в названии которого фигурирует название столицы страны
                        string cityIdSearch = "SELECT id FROM Cities WHERE name = '" + country.capital + "'";
                        SqlCommand cityIdSearchCommand = new SqlCommand(cityIdSearch, sqlConnection);
                        cityIdSearchCommand.Connection.Open();
                        using (DbDataReader cityIdReader = cityIdSearchCommand.ExecuteReader())
                        {
                            // Если есть результаты
                            if (cityIdReader.HasRows)
                            {
                                //  Заносим в cityId первый результат поиска города
                                cityIdReader.Read();
                                cityId = cityIdReader.GetInt32(cityIdReader.GetOrdinal("id"));
                                cityIdSearchCommand.Connection.Close();
                            }
                            // Если города не существует
                            else
                            {
                                // В базе данных создаётся новый город и извлекается cityId 
                                // нового города
                                string cityInsert = "INSERT INTO Cities (name) " +
                                    "VALUES ('" + country.capital + "'); " +
                                    "SELECT CAST(scope_identity() AS int)";
                                SqlCommand cityInsertCommand = new SqlCommand(cityInsert, sqlConnection);
                                cityIdSearchCommand.Connection.Close();
                                cityInsertCommand.Connection.Open();
                                cityId = (Int32)cityInsertCommand.ExecuteScalar();
                                cityInsertCommand.Connection.Close();
                            }
                        }

                        // Ищет в базе данных регион, в названии которого фигурирует название региона
                        // в котором страна расположена
                        string regionIdSearch = "SELECT id FROM Regions " +
                            "WHERE name = '" + country.region + "'";
                        SqlCommand regionIdSearchCommand = new SqlCommand(regionIdSearch, sqlConnection);
                        regionIdSearchCommand.Connection.Open();
                        using (DbDataReader regionIdReader = regionIdSearchCommand.ExecuteReader())
                        {
                            // Если есть результаты
                            if (regionIdReader.HasRows)
                            {
                                // Заносим в regionId первый результат поиска региона
                                regionIdReader.Read();
                                regionId = regionIdReader.GetInt32(regionIdReader.GetOrdinal("id"));
                                regionIdSearchCommand.Connection.Close();
                            }
                            // Если региона не существует
                            else
                            {
                                // В базе данных создаётся новый регион и извлекается regionId 
                                // нового региона
                                string regionInsert = "INSERT INTO Regions (name) " +
                                    "VALUES ('" + country.region + "'); " +
                                    "SELECT CAST(scope_identity() AS int)";
                                SqlCommand regionInsertCommand = new SqlCommand(regionInsert,
                                    sqlConnection);
                                regionIdSearchCommand.Connection.Close();
                                regionInsertCommand.Connection.Open();
                                regionId = (Int32)regionInsertCommand.ExecuteScalar();
                                regionInsertCommand.Connection.Close();
                            }
                        }

                        // Ищет в базе данных страну по коду страны
                        string countryIdSearch = "SELECT id FROM Countries " +
                            "WHERE code = '" + country.code + "'";
                        SqlCommand countryIdSearchCommand = new SqlCommand(countryIdSearch, sqlConnection);
                        countryIdSearchCommand.Connection.Open();

                        using (DbDataReader countryIdReader = countryIdSearchCommand.ExecuteReader())
                        {
                            // Если есть результаты поиска
                            if (countryIdReader.HasRows)
                            {
                                // Обновляются данные страны по её найденному id
                                countryIdReader.Read();
                                countryId = countryIdReader.GetInt32(countryIdReader.GetOrdinal("id"));
                                string countryUpdate = "UPDATE Countries SET " +
                                    "name = '" + country.name + "', capital = " + cityId + ", " +
                                    "area = " + country.area + ", " +
                                    "population = " + country.population + ", " +
                                    "region = " + regionId + " WHERE id = " + countryId + "";
                                SqlCommand countryUpdateCommand = new SqlCommand(countryUpdate,
                                    sqlConnection);
                                countryIdSearchCommand.Connection.Close();
                                countryUpdateCommand.Connection.Open();
                                countryUpdateCommand.ExecuteNonQuery();
                                countryUpdateCommand.Connection.Close();
                                MessageBox.Show(
                                    "В базу данных внесены изменения по стране " + textBox7.Text,
                                    "Сообщение",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1,
                                    MessageBoxOptions.DefaultDesktopOnly);
                            }
                            // Если страны не существует в базе данных
                            else
                            {
                                // В базу данных заносится новая страна
                                string countryInsert = "INSERT INTO Countries " +
                                    "(name, code, capital, area, population, region) " +
                                    "VALUES ('" + country.name + "', '" + country.code + "', " +
                                    "" + cityId + ", " + country.area + ", " +
                                    "" + country.population + ", " + regionId + ")";
                                SqlCommand countryInsertCommand = new SqlCommand(countryInsert,
                                    sqlConnection);
                                countryIdSearchCommand.Connection.Close();
                                countryInsertCommand.Connection.Open();
                                countryInsertCommand.ExecuteNonQuery();
                                countryInsertCommand.Connection.Close();
                                MessageBox.Show(
                                    "Информация о новой стране " + country.name + " внесена в базу данных",
                                    "Сообщение",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1,
                                    MessageBoxOptions.DefaultDesktopOnly);
                            }
                        }
                    }
                }
                // Если connectionString пуст
                catch (InvalidOperationException exception)
                {
                    MessageBox.Show("Невозможно подключиться в базе данных. " +
                    "Уточните конфигурации подключения к базе данных в настройках",
                    "Внимание",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                }
                // Если connectionString содержит некорректные данные, по которым не удаётся
                // подключиться к базе данных
                catch (SqlException exception)
                {
                    MessageBox.Show("Невозможно подключиться в базе данных. " +
                        "Уточните конфигурации подключения к базе данных в настройках",
                        "Внимание",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                }
            }
        }
    }
}
