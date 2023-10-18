using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interface
{
    public partial class TableSalaryEmployee : Form
    {
        int nam = 2023;
        int thang = 8;
        private string conStr = "Data Source=highlanddb.database.windows.net;Initial Catalog=highland;User ID=vudance;Password=Vudang0402;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public TableSalaryEmployee()
        {
            InitializeComponent();
        }

        private void TableSalaryEmployee_Load(object sender, EventArgs e)
        {
            guna2ComboBox1.SelectedItem = "2023";
            guna2ComboBox2.SelectedItem = "8";
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;


            SqlConnection connection = new SqlConnection(conStr);

            try
            {
                connection.Open();
                string query = "SELECT * FROM TableSalary WHERE Year = 2023 AND Month = 8";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối đến cơ sở dữ liệu: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }


        }



        public List<Employee> GetEmployees()
        {
            List<Employee> listEmployee = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM Employee";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee employee = new Employee
                            {
                                Name = (string)reader["Name"],

                            };
                            listEmployee.Add(employee);
                        }
                    }
                }
            }

            return listEmployee;
        }

        private int GetDayNumber(int thang, int nam)
        {
            int dayNumber = 0;
            switch (thang)
            {
                case 2:
                    dayNumber = (nam % 4 == 0 && nam % 100 != 0) || nam % 400 == 0 ? 29 : 28;
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    dayNumber = 30;
                    break;
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    dayNumber = 31;
                    break;
            }
            return dayNumber;
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(conStr);

            var listEmployee = GetEmployees();
            if (listEmployee.Count == 0)
            {
                return;
            }
            foreach (var item in listEmployee)
            {
                List<string> listDay = new List<string>();
                for (int j = 1; j <= GetDayNumber(thang, nam); j++)
                {
                    DateTime date = new DateTime(nam, thang, j);
                    switch (date.DayOfWeek.ToString())
                    {
                        case "Sunday":
                            listDay.Add("CN");
                            break;
                        case "Saturday":
                            listDay.Add("T7");
                            break;
                        default:
                            listDay.Add("X");
                            break;
                    }
                    switch (listDay.Count)
                    {
                        case 28:
                            listDay.Add("");
                            listDay.Add("");
                            listDay.Add("");
                            break;
                        case 29:
                            listDay.Add("");
                            listDay.Add("");
                            break;
                        case 30:
                            listDay.Add("");
                            break;
                    }

                }
                string insertQuery = "INSERT INTO TableSalary (Name, D1, D2, D3, D4, D5, D6, D7, D8, D9, D10, D11, D12, D13, D14, D15, D16, D17, D18, D19, D20, D21, D22, D23, D24, D25, D26, D27, D28, D29, D30, D31, Month, Year) VALUES (@Name, @D1, @D2, @D3, @D4, @D5, @D6, @D7, @D8, @D9, @D10, @D11, @D12, @D13, @D14, @D15, @D16, @D17, @D18, @D19, @D20, @D21, @D22, @D23, @D24, @D25, @D26, @D27, @D28, @D29, @D30, @D31, @Month, @Year)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open(); // Mở kết nối chỉ khi nó đã đóng.
                    }

                    command.Parameters.AddWithValue("@Name", item.Name);
                    for (int i = 0; i < listDay.Count; i++)
                    {
                        command.Parameters.AddWithValue($"@D{i + 1}", listDay[i]);
                    }
                    command.Parameters.AddWithValue("@Month", int.Parse(guna2ComboBox2.Text));
                    command.Parameters.AddWithValue("@Year", int.Parse(guna2ComboBox1.Text));
                    command.ExecuteNonQuery();
                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            //{
            //    string cellValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            //    if (cellValue == "T7        ")
            //    {
            //        e.CellStyle.BackColor = Color.Orange;
            //    }
            //    if(cellValue=="CN        ")
            //    {
            //        e.CellStyle.BackColor = Color.OrangeRed;
            //    }
            //}
        }
    }
}
