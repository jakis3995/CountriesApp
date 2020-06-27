using System;
using System.Windows.Forms;

namespace Countries
{
    class ConfigChecker
    {
        public string[] Check(string connectionString)
        {
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
                catch (Exception exception)
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
                catch (Exception exception)
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
                    catch (Exception exception)
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
                    catch (Exception exception)
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
                    catch (Exception exception)
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
