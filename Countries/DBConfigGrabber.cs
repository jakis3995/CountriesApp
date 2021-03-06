﻿using System;
using System.IO;
using System.Windows.Forms;

namespace Countries
{
    class DbConfigGrabber : IDbConfigGrabber
    {
        /* Класс, получающий информацию по подключению к базе данных из файла
         */
        public String GetConnectionString(string fileName)
        {
            /* Метод, получающий connectionString из файла
             * Если файл не найден или в нём отсуствуют записи, выведутся соответствующие
             * сообщения
             */
            string connectionString = null;

            if (File.Exists(fileName))
            {
                FileStream configFile = File.OpenRead(fileName);

                StreamReader streamReader = new StreamReader(configFile);
                if (configFile.Length > 0)
                {
                    string redLine = streamReader.ReadLine();
                    // Убирает последнюю точку с запятой, если таковая есть
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

            return connectionString;
        }
    }
}
