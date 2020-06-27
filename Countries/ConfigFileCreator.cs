using System.IO;
using System.Windows.Forms;

namespace Countries
{
    class ConfigFileCreator
    {
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
            catch (IOException exception)
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
