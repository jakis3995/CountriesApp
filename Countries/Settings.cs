using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Countries
{
    public partial class Settings : Form
    {
        private Form1 form1;
        public Settings(Form1 form1)
        {
            this.form1 = form1;
            InitializeComponent();
            /*
             *    При инициализации формы "Настройки" в поля ввода "Имя сервера", "Имя базы данных"
             * и, если в файле конфигурации подключения фигурируют записи о имени пользователя и 
             * пароле, то в поля ввода "Имя пользователя" и "Пароль" (ставится галочка о том, что
             * аутентификация требуется) - заносятся соответствующие данные, взятые из 
             * connectionString. При этом данные проверяются
             */
            if (this.form1.connectionString != null)
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
                string[] configurationProperties = this.form1.connectionString.Split(';');
                if (!(configurationProperties.Length == 3 || configurationProperties.Length == 4))
                {
                    fatallyCorruptedFileFlag = true;
                }
                else
                {
                    try
                    {
                        textBox1.Text = configurationProperties[0].Split('=')[1];
                        if (!configurationProperties[0].Split('=')[0].Equals("Data Source"))
                        {
                            corruptedFileFlag = true;
                        }
                    }
                    catch (Exception exception)
                    {
                        fatallyCorruptedFileFlag = true;
                    }
                    try
                    {
                        textBox2.Text = configurationProperties[1].Split('=')[1];
                        if (!configurationProperties[1].Split('=')[0].Equals("Initial Catalog"))
                        {
                            corruptedFileFlag = true;
                        }
                    }
                    catch (Exception exception)
                    {
                        fatallyCorruptedFileFlag = true;
                    }
                    if (configurationProperties.Length == 4)
                    {
                        checkBox1.Checked = true;
                        try
                        {
                            textBox3.Text = configurationProperties[2].Split('=')[1];
                            if (!configurationProperties[2].Split('=')[0].Equals("User Id"))
                            {
                                corruptedFileFlag = true;
                            }
                        }
                        catch (Exception exception)
                        {
                            fatallyCorruptedFileFlag = true;
                        }
                        try
                        {
                            textBox4.Text = configurationProperties[3].Split('=')[1];
                            if (!configurationProperties[3].Split('=')[0].Equals("Password"))
                            {
                                corruptedFileFlag = true;
                            }
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
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*
             * Обработка нажатия на кнопку "Назад к меню"
             */
            Visible = false;
            form1.Visible = true;
        }

        string connectionString;

        private bool IsConnectionTestSuccessful()
        {
            /*
             *    Метод проверяет может ли приложение подключится к базе данных по введённым
             * в текстовые поля данным конфигурации подключения. Возвращает логическое значение:
             * true - если подключение удалось.
             */
            bool success = false;
            string dataSource = textBox1.Text;
            string databaseName = textBox2.Text;
            StringBuilder connectionStringBuilder = new StringBuilder();
            connectionStringBuilder.Append("Data Source=" + dataSource + ";");
            connectionStringBuilder.Append("Initial Catalog=" + databaseName + ";");
            if (checkBox1.Checked)
            {
                connectionStringBuilder.Append("User Id=" + textBox3.Text + ";");
                connectionStringBuilder.Append("Password=" + textBox4.Text);
            }
            else
            {
                connectionStringBuilder.Append("Integrated Security=True");
            }
            connectionString = connectionStringBuilder.ToString();
            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                sqlConnection.Close();
                
                success = true;
            }
            catch (Exception exception)
            {
            }

            return success;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*
             * Обработка нажатия на кнопку "Тест подключения"
             */
            if (IsConnectionTestSuccessful())
            {
                MessageBox.Show("Удалось подключиться к базе данных",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                MessageBox.Show("Не удалось подключиться к базе данных. Проверьте введённые данные",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            form1.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            /*
             *    Обработка изменения галочки "Требуется?[аутентификация при подключении]"
             *    Если галочка отсутсвует в боксе, скрывает текстовые поля для ввода имени 
             * пользователя и пароля, и при при сохранении данных конфигурации подключения в 
             * качестве третьего параметра ставит "Integrated Security=True". 
             *    Если присутствует - показывает текстовые поля для ввода имени пользователя и
             * пароля, и при сохранении данных конфигурации подключения в качестве третьего 
             * параметра ставит имя пользователя и пароль.
             */
            if (checkBox1.Checked)
            {
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                checkBox2.Enabled = true;
            }
            else
            {
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                checkBox2.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            /*
             * Обработка изменения галочки "Показать пароль"
             */
            if (checkBox2.Checked)
            {
                textBox4.PasswordChar = '\0';
            }
            else
            {
                textBox4.PasswordChar = '*';
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
             *    Обработка нажатия на кнопку "Сохранить изменения".
             *    Проверяет возможность подключения и после успешного теста переписывает файл 
             * "connectionConfig.txt" с новыми данными. Также предлагает перезапустить программу,
             * так как, connectionString, нужный на протяжении почти всей работы программы,
             * обновляется только при запуске программы. Таким образом, программа станет 
             * взаимодействовать с новой базой данных только при перезапуске программы. 
             * Если пользователь продолжит пользоваться программой без перезапуска, при 
             * сохранении/извлечении данных из базы данных будет использоваться старая база 
             * данных, connectionString с которой связан с самого начала работы всей программы.
             *    Сама connectionString берётся после проведения теста 
             * (IsConnectionTestSuccessful()) на подключение к базе данных с введёнными 
             * пользователем параметрами.
             */
            if (IsConnectionTestSuccessful())
            {
                using (FileStream configFile = File.Create("connectionConfig.txt"))
                {
                    StreamWriter streamWriter = new StreamWriter(configFile);
                    streamWriter.WriteLine(connectionString);
                    streamWriter.Close();
                }
                MessageBox.Show("Чтобы изменения вступили в силу, перезапустите программу",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                MessageBox.Show("Не удалось подключиться к базе данных. Проверьте введённые данные",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
        }
    }
}
