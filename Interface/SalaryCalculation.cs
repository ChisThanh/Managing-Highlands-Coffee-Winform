using Interface.Helpers;
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
    public partial class SalaryCalculation : Form
    {
        private string conStr = "Data Source=highlanddb.database.windows.net;Initial Catalog=highland;User ID=vudance;Password=Vudang0402;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public SalaryCalculation()
        {

            InitializeComponent();

        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           


        }
        public List<Dictionary<string, object>> GetQueryResult(string sqlQuery)
        {
            List<Dictionary<string, object>> resultList = new List<Dictionary<string, object>>();

            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object columnValue = reader[i];
                                row[columnName] = columnValue;
                            }

                            resultList.Add(row);
                        }
                    }
                }
            }

            return resultList;
        }

        public int CalculateSalary(string role)
        {
            int salary = 0;

            if (role.Contains("Sale"))
            {
                salary = 8000000;
            }
            else if (role.Contains("Officer"))
            {
                salary = 12000000;
            }

            return salary;
        }
        public int CalculateTroCap(string role)
        {
            int salary = 0;

            if (role.Contains("Sale"))
            {
                salary = 300000;
            }
            else if (role.Contains("Officer"))
            {
                salary = 500000;
            }

            return salary;
        }
        private CalculateDay CalculateDayOfEmpolyee(SqlConnection connection, string employeeName)
        {
            string selectedYear = guna2ComboBox1.SelectedItem.ToString();
            string selectedMonth = guna2ComboBox2.SelectedItem.ToString();
            string query = $"SELECT D1, D2, D3, D4, D5, D6, D7, D8, D9, D10, D11, D12, D13, D14, D15, D16, D17, D18, D19, D20, D21, D22, D23, D24, D25, D26, D27, D28, D29, D30, D31 FROM TableSalary WHERE Name = @EmployeeName AND Year = {selectedYear} AND Month = {selectedMonth}";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@EmployeeName", employeeName);

                CalculateDay calculateDay = new CalculateDay();

                SqlDataReader reader = cmd.ExecuteReader();
                int countV = 0;
                int countX = 0;

                while (reader.Read())
                {
                    for (int i = 0; i < 31; i++)
                    {
                        if (reader.GetString(i).Contains("V"))
                        {
                            countV++;
                        }
                        else if (reader.GetString(i).Contains("X"))
                        {
                            countX++;
                        }
                    }
                }
                calculateDay.CountV = countV;
                calculateDay.CountX = countX;
                reader.Close();
                return calculateDay;
            }
        }
        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    await connection.OpenAsync();

                    string query = $"SELECT top 20 Id,Name,Role  FROM Employee ";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    await Task.Run(() => adapter.Fill(dataTable));

                    dataTable.Columns["Role"].ColumnName = "Chức vụ";
                    dataTable.Columns["Name"].ColumnName = "Họ và tên";
                    
                    dataTable.Columns.Add("Lương cơ bản", typeof(int));
                    dataTable.Columns.Add("Ngày nghỉ", typeof(int));
                    dataTable.Columns.Add("Ngày công", typeof(int));
                    dataTable.Columns.Add("Trợ cấp", typeof(int));
                    dataTable.Columns.Add("Thực lãnh", typeof(int));

                    foreach (DataRow row in dataTable.Rows)
                    {

                        string role = row["Chức vụ"].ToString();
                        int salary = CalculateSalary(role);
                        row["Lương cơ bản"] = salary;
                        string employeeName = row["Họ và tên"].ToString();
                        int ngaynghi = CalculateDayOfEmpolyee(connection, employeeName).CountV;
                        row["Ngày nghỉ"] = ngaynghi;
                        int ngaycong = CalculateDayOfEmpolyee(connection, employeeName).CountX;
                        row["Ngày công"] = ngaycong;
                        row["Trợ cấp"] = CalculateTroCap(role);
                        row["Thực lãnh"] = salary - ngaynghi * 300000 +300000;
                    }
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private async void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    await connection.OpenAsync();

                    string query = $"SELECT Id,Name,Role  FROM Employee ";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    await Task.Run(() => adapter.Fill(dataTable));

                    dataTable.Columns["Role"].ColumnName = "Chức vụ";
                    dataTable.Columns["Name"].ColumnName = "Họ và tên";

                    dataTable.Columns.Add("Lương cơ bản", typeof(int));
                    dataTable.Columns.Add("Ngày nghỉ", typeof(int));
                    dataTable.Columns.Add("Ngày công", typeof(int));
                    dataTable.Columns.Add("Trợ cấp", typeof(int));
                    dataTable.Columns.Add("Thực lãnh", typeof(int));

                    foreach (DataRow row in dataTable.Rows)
                    {

                        string role = row["Chức vụ"].ToString();
                        int salary = CalculateSalary(role);
                        row["Lương cơ bản"] = salary;
                        string employeeName = row["Họ và tên"].ToString();
                        int ngaynghi = CalculateDayOfEmpolyee(connection, employeeName).CountV;
                        row["Ngày nghỉ"] = ngaynghi;
                        int ngaycong = CalculateDayOfEmpolyee(connection, employeeName).CountX;
                        row["Ngày công"] = ngaycong;
                        int trocap = CalculateTroCap(role);
                        row["Trợ cấp"] = trocap;
                        row["Thực lãnh"] = salary - ngaynghi * 300000 + 300000;
                    }
                     
                    Excel excel = new Excel();
                    excel.ExportFile(dataTable, "Bảng lương", "Bảng lương nhân viên");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
    
}
