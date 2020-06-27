using System;
using System.Windows.Forms;

namespace Countries
{
    public partial class SavedCountriesForm : Form
    {
        /* Класс формы отображения записей базы данных (сохраненных стран) в таблице
         */
        private Form1 form1;
        string connectionString;
        public SavedCountriesForm(Form1 form1)
        {
            this.form1 = form1;
            InitializeComponent();
            connectionString = form1.GetConnectionString();
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
            DbRequester dbRequester = new DbRequester(connectionString);
            // Проверяет возможность подключения к базе данных
            int connectionErrorCode = dbRequester.CreateConnection();
            // Через экземпляр класса DbRequester получает таблицу стран и заполняет ею dataGridView1
            if (connectionErrorCode == 0)
            {
                dataGridView1.DataSource = dbRequester.GetCountriesTable();
            }
            else
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
