using System;
using System.Windows.Forms;

namespace Countries
{
    public partial class SettingsForm : Form
    {
        /* Класс формы настроек подключения к базе данных
         */
        private Form1 form1;
        private string connectionString;
        public SettingsForm(Form1 form1)
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
            if (this.form1.GetConnectionString() != null)
            {
                /*
                 *    Если файл сильно повреждается: отсутствуют знаки "=;" или слишком много 
                 * или мало параметров - флаг fatallyCorruptedFileFlag (в методе Check из класса
                 * configChecker) ставится на true и данные не заносятся в текстовые поля.
                 *    Если файл незначительно повреждается: названия параметров конфигурации
                 * не являются корректными - флаг corruptedFileFlag ставится на true и данные, 
                 * которые можно прочитать, заносятся в соответствующие текстовые поля.
                 *    В конце, если стоит fatallyCorruptedFileFlag предлагается уточнить данные
                 * конфигурации подключения; если стоит corruptedFileFlag предлагается
                 * пересохранить файл конфигурации.
                 */
                IConfigChecker configChecker = new ConfigChecker();
                string[] configurationProperties = configChecker.Check(this.form1.GetConnectionString());
                if (configurationProperties.Length == 3)
                {
                    textBox1.Text = configurationProperties[0];
                    textBox2.Text = configurationProperties[1];
                }
                else
                {
                    if (configurationProperties.Length == 4)
                    {
                        checkBox1.Checked = true;
                        textBox1.Text = configurationProperties[0];
                        textBox2.Text = configurationProperties[1];
                        textBox3.Text = configurationProperties[2];
                        textBox4.Text = configurationProperties[3];
                    }
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

        private void button2_Click(object sender, EventArgs e)
        {
            /*
             * Обработка нажатия на кнопку "Тест подключения"
             */
            IDbConnectionTester dbConnectionTester = new DbConnectionTester();
            connectionString = dbConnectionTester.ConnectionStringBuilder(textBox1.Text,
                textBox2.Text, checkBox1.Checked, textBox3.Text,
                textBox4.Text);
            if (dbConnectionTester.IsConnectionTestSuccessful(connectionString))
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
             * (IsConnectionTestSuccessful() в DbConnectionTester) на подключение к базе данных
             * с введёнными пользователем параметрами.
             */
            IDbConnectionTester dbConnectionTester = new DbConnectionTester();
            connectionString = dbConnectionTester.ConnectionStringBuilder(textBox1.Text,
                textBox2.Text, checkBox1.Checked, textBox3.Text,
                textBox4.Text);
            if (dbConnectionTester.IsConnectionTestSuccessful(connectionString))
            {
                IConfigFileCreator configFileCreator = new ConfigFileCreator();
                configFileCreator.CreateConfigFile(connectionString);
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
