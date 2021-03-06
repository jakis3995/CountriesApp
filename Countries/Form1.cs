﻿using System;
using System.Windows.Forms;

namespace Countries
{
    public partial class Form1 : Form
    {
        /* Класс формы главного меню программы
         */
        private CountriesSearchForm countriesSearchForm;
        private SavedCountriesForm savedCountriesForm;
        private SettingsForm settingsForm;
        private string connectionString;

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

            string fileName = "connectionConfig.txt";
            IDbConfigGrabber dbConfigGrabber = new DbConfigGrabber();
            connectionString = dbConfigGrabber.GetConnectionString(fileName);

            /* Создаются формы, которые будут доступны на всём протяжении работы программы
             */
            countriesSearchForm = new CountriesSearchForm(this)
            {
                Visible = false
            };
            savedCountriesForm = new SavedCountriesForm(this)
            {
                Visible = false
            };
            settingsForm = new SettingsForm(this)
            {
                Visible = false
            };
        }

        public string GetConnectionString()
        {
            return this.connectionString;
        }

        /* Переход по формам через кнопки меню
         */
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
