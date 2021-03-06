﻿using System.IO;
using System.Windows.Forms;

namespace Countries
{
    class ConfigFileCreator : IConfigFileCreator
    {
        /* Класс, создающий файл конфигурации подключения к базе данных
         */
        private string fileName = "connectionConfig.txt";

        public void CreateConfigFile(string connectionString)
        {
            try
            {
                FileStream configFile = File.Create(fileName);
                StreamWriter streamWriter = new StreamWriter(configFile);
                streamWriter.WriteLine(connectionString);
                streamWriter.Close();
            }
            catch (IOException)
            {
                MessageBox.Show("Файл конфигурации занят другим процессом. Закройте программы" +
                                ", которые могли бы использовать данный файл и попробуйте сохранить" +
                                " заново",
                    "Внимание",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
        }
    }
}
