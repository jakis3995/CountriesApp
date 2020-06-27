using System;
using System.Windows.Forms;

namespace Countries
{
    public partial class SavedCountriesForm : Form
    {
        private Form1 form1;
        string connectionString;
        public SavedCountriesForm(Form1 form1)
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
            DbRequester dbRequester = new DbRequester(connectionString);
            int connectionErrorCode = dbRequester.CreateConnection();
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
