using System;
using System.Threading;
using System.Windows.Forms;

namespace Countries
{
    public partial class CountriesSearchForm : Form
    {
        private Form1 form1;
        string connectionString;
        Country country;
        public CountriesSearchForm(Form1 form1)
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
                CountryInfoGrabber countryInfoGrabber = new CountryInfoGrabber(url);
                int errorCode = countryInfoGrabber.CheckGetAbility(countryName);

                switch (errorCode)
                {
                    case 0:
                        country = countryInfoGrabber.GetCountryInfo(countryName);
                        textBox2.Text = country.code;
                        textBox3.Text = country.capital;
                        textBox4.Text = country.area.ToString();
                        textBox5.Text = country.population.ToString();
                        textBox6.Text = country.region;
                        textBox7.Text = country.name;
                        break;
                    case -1:
                        MessageBox.Show("Неизвестная ошибка при попытке найти информацию" +
                                        "по стране во внешней базе стран",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    case 1:
                        MessageBox.Show(
                            "Введено неверное название страны. Пожалуйста, перепроверьте " +
                            "введённое название страны. Обратите внимание, название страны " +
                            "должно быть указано на английском языке",
                            "Сообщение",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    case 2:
                        MessageBox.Show(
                            "Проблемы с Интернет-соединением. Проверьте подключение",
                            "Сообщение",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        break;
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
                        MessageBox.Show("В базу данных внесена информация о новой стране",
                            "Сообщение",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else
                    {
                        dbConnection.UpdateCountry(country, cityId, regionId, countryId);
                        MessageBox.Show("В базе данных внесены изменения по введённой" +
                                        "стране",
                            "Сообщение",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                    }
                }
                else
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
