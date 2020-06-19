using System;
using System.IO;
using System.Windows.Forms;

namespace Countries
{
    public partial class Form1 : Form
    {
        private CountriesSearch countriesSearchForm;
        private SavedCountries savedCountriesForm;
        private Settings settingsForm;
        public string connectionString;

        public Form1()
        {
            InitializeComponent();

            /*
             *    При создании заглавной формы приложения производится поиск файла конфигурации 
             * подключения к базе данных, который находится в одной папке с исполняемым файлом 
             * приложения. Если данный файл есть, захватывается connectionString, который потом
             * уже используется на протяжении всей программы. Если файл отсутствует или, в нём 
             * отсутствуют записи, выводятся соответствующие сообщения.
             */
            if (File.Exists("connectionConfig.txt"))
            {
                FileStream configFile = File.OpenRead("connectionConfig.txt");

                StreamReader streamReader = new StreamReader(configFile);
                if (configFile.Length > 0)
                {
                    string redLine = streamReader.ReadLine();
                    if (redLine.EndsWith(";"))
                    {
                        redLine = redLine.Substring(0, redLine.Length - 2);
                    }
                    connectionString = redLine;
                }
                else
                {
                    MessageBox.Show("В файле конфигурации подключения к базе данных отсутствуют записи. " +
                    "Зайдите в настройки для конфигурации подключения",
                    "Внимание",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            else
            {
                MessageBox.Show("Файл конфигурации подключения к базе данных не найден. " +
                    "Зайдите в настройки для конфигурации подключения",
                    "Внимание",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }

            countriesSearchForm = new CountriesSearch(this)
            {
                Visible = false
            };
            savedCountriesForm = new SavedCountries(this)
            {
                Visible = false
            };
            settingsForm = new Settings(this)
            {
                Visible = false
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            countriesSearchForm.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            savedCountriesForm.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            settingsForm.Visible = true;
        }
    }
}
