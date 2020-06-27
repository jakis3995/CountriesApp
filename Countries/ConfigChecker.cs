using System;
using System.Windows.Forms;

namespace Countries
{
    class ConfigChecker : IConfigChecker
    {
        /* Класс, проверяющий файл конфигурации подключения к базе данных на корректность
         */
        public string[] Check(string connectionString)
        {
            /*
             *    Если файл сильно повреждается: отсутствуют знаки "=;" или слишком много 
             * или мало параметров - флаг fatallyCorruptedFileFlag ставится на true и данные
             * не заносятся в текстовые поля.
             *    Если файл незначительно повреждается: названия параметров конфигурации
             * не являются корректными - флаг corruptedFileFlag ставится на true и данные, 
             * которые можно прочитать, заносятся в соответствующие текстовые поля.
             *    В конце, если стоит fatallyCorruptedFileFlag предлагается уточнить данные
             * конфигурации подключения; если стоит corruptedFileFlag предлагается
             * пересохранить файл конфигурации.
             */

            bool fatallyCorruptedFileFlag = false, corruptedFileFlag = false;
            string[] configurationProperties = connectionString.Split(';');
            if (!(configurationProperties.Length == 3 || configurationProperties.Length == 4))
            {
                fatallyCorruptedFileFlag = true;
            }
            else
            {
                try
                {
                    if (!configurationProperties[0].Split('=')[0].Equals("Data Source"))
                    {
                        corruptedFileFlag = true;
                    }
                    configurationProperties[0] = configurationProperties[0].
                        Split('=')[1];
                }
                catch (Exception)
                {
                    fatallyCorruptedFileFlag = true;
                }
                try
                {
                    if (!configurationProperties[1].Split('=')[0].Equals("Initial Catalog"))
                    {
                        corruptedFileFlag = true;
                    }
                    configurationProperties[1] = configurationProperties[1].Split('=')[1];
                }
                catch (Exception)
                {
                    fatallyCorruptedFileFlag = true;
                }
                if (configurationProperties.Length == 4)
                {
                    try
                    {
                        if (!configurationProperties[2].Split('=')[0].Equals("User Id"))
                        {
                            corruptedFileFlag = true;
                        }
                        configurationProperties[2] = configurationProperties[2].Split('=')[1];
                    }
                    catch (Exception)
                    {
                        fatallyCorruptedFileFlag = true;
                    }
                    try
                    {
                        if (!configurationProperties[3].Split('=')[0].Equals("Password"))
                        {
                            corruptedFileFlag = true;
                        }
                        configurationProperties[3] = configurationProperties[3].Split('=')[1];
                    }
                    catch (Exception)
                    {
                        fatallyCorruptedFileFlag = true;
                    }
                }
                else
                {
                    try
                    {
                        if (!configurationProperties[2].Split('=')[0].Equals("Integrated Security") &&
                            (!configurationProperties[2].Split('=')[1].Equals("True")))
                        {
                            corruptedFileFlag = true;
                        }
                    }
                    catch (Exception)
                    {
                        corruptedFileFlag = true;
                    }
                }
            }
            if (fatallyCorruptedFileFlag)
            {
                MessageBox.Show("Файл конфигурации подключения к базе данных сильно повреждён. " +
                    "Уточните данные для подключения в настройках",
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
            }
            if (corruptedFileFlag)
            {
                MessageBox.Show("Файл конфигурации подключения к базе данных повреждён. " +
                    "Пересохраните данные конфигурации в настройках",
                "Внимание",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
            }
            return configurationProperties;
        }
    }
}
