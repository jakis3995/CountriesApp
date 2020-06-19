using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Countries
{
    public partial class SavedCountries : Form
    {
        private Form1 form1;
        string connectionString;
        public SavedCountries(Form1 form1)
        {
            this.form1 = form1;
            InitializeComponent();
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

        private void SavedCountries_FormClosed(object sender, FormClosedEventArgs e)
        {
            form1.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*
             * Обработка нажатия на кнопку "Заполнить таблицу данными из базы данных"
             */
            try
            {
                /*
                 *    Проверяется, возможно ли произвести соединение с базой данных. 
                 *    Если да, 
                 * таблица dataGridView1 заполняется данными из запроса вывода всех стран 
                 * из базе данных, при этом выводятся наименование страны, её код, наименование
                 * столицы, площадь, численность населения, а также наименование региона.
                 *    Если нет, 
                 *    - если connectionString ничего в себе не хранит (при запуске приложения
                 *       не был найден файл конфигурации подключения к базе данных, либо он пуст), 
                 *       а это InvalidOperationException -
                 *    - если в connectionString содержатся некорректные данные, по которым
                 *       подключиться к базе данных не получится, а это SqlException -
                 * } в обоих случаях выводится сообщение о том, что невозможно подключиться к 
                 * } базе данных, и что необходимо уточнить данные для подключения в настройках.
                 */
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    string dataQuery = "SELECT c.name AS 'Название', c.code AS 'Код страны', " +
                        "Cities.name AS 'Столица', c.area AS 'Площадь', " +
                        "c.population AS 'Население', Regions.name AS 'Регион' " +
                        "FROM Countries c INNER JOIN Cities on c.capital = Cities.id " +
                        "INNER JOIN Regions on c.region = Regions.id";
                    SqlDataAdapter dataQueryCommand = new SqlDataAdapter(dataQuery, sqlConnection);
                    DataTable dataTable = new DataTable();
                    dataQueryCommand.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    sqlConnection.Close();
                }
            }
            catch (InvalidOperationException exception)
            {
                MessageBox.Show("Невозможно подключиться в базе данных. " +
                    "Уточните конфигурации подключения к базе данных в настройках",
                    "Внимание",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
            catch (SqlException exception)
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
