using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interface
{
    public partial class Employee : Form
    {
        private string conStr = "Data Source=highlanddb.database.windows.net;" +
                                "Initial Catalog=highland;User ID=vudance;" +
                                "Password=Vudang0402;" +
                                "Connect Timeout=60;" +
                                "Encrypt=True;" +
                                "TrustServerCertificate=False;" +
                                "TrustServerCertificate=False;" +
                                "ApplicationIntent=ReadWrite;" +
                                "MultiSubnetFailover=False";
        public Employee()
        {
            InitializeComponent();
        }
        private async void Employee_Load(object sender, EventArgs e)
        {
            guna2TextBox4.Text = "1";
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    await connection.OpenAsync();

                    string query = "SELECT top 10 * FROM Employee";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    await Task.Run(() => adapter.Fill(dataTable));

                    guna2DataGridView1.DataSource = dataTable;
                    guna2DataGridView1.Columns["Avatar"].Visible = false;

                    guna2DataGridView1.Columns["Name"].HeaderText = "Tên";
                    guna2DataGridView1.Columns["Age"].HeaderText = "Tuổi";
                    guna2DataGridView1.Columns["CreatedAt"].HeaderText = "Ngày vào làm";
                    guna2DataGridView1.Columns["role"].HeaderText = "Chức vụ";

                    foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
                    {
                        column.ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            string filePath = "C:/Users/nhuye/Downloads/data.txt";

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);


                foreach (string line in lines)
                {
                    string[] values = line.Split(',');
                    guna2DataGridView1.Rows.Add(values);
                }

            }
            else
            {
                MessageBox.Show("File không tồn tại!");
            }
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {

            string name = guna2TextBox1.Text;
            int age = int.Parse(guna2TextBox2.Text);
            string role = comboBox1.Text;
            byte[] imagePath = getImage();

            InsertData(name, age, role, imagePath);


        }
        private byte[] getImage()
        {
            using (var memoryStream = new MemoryStream())
            {
                guna2PictureBox1.Image.Save(memoryStream, guna2PictureBox1.Image.RawFormat);
                return memoryStream.ToArray();
            }
        }
        private void InsertData(string name, int age, string role, byte[] imagePath)
        {
            try
            {
                string connectionString = conStr;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO Employee (Name, Age, Role, Avatar, CreatedAt) VALUES (@Name, @Age, @Role, @Avatar, @CreatedAt)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Age", age);
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@Avatar", imagePath);
                        cmd.Parameters.AddWithValue("@CreatedAt", guna2DateTimePicker1.Value);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Dữ liệu đã được thêm vào cơ sở dữ liệu.");
                        ReloadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        private void guna2Button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                guna2PictureBox1.Image = new Bitmap(openFileDialog.FileName);
            }
        }
        private async void ReloadData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    await connection.OpenAsync();

                    string query = "SELECT top 10 * FROM Employee";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    await Task.Run(() => adapter.Fill(dataTable));

                    guna2DataGridView1.Invoke((Action)(() =>
                    {
                        guna2DataGridView1.DataSource = dataTable;
                        guna2DataGridView1.Columns["Avatar"].Visible = false;

                        guna2DataGridView1.Columns["Name"].HeaderText = "Tên";
                        guna2DataGridView1.Columns["Age"].HeaderText = "Tuổi";
                        guna2DataGridView1.Columns["CreatedAt"].HeaderText = "Ngày vào làm";
                        guna2DataGridView1.Columns["role"].HeaderText = "Chức vụ";

                        foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
                        {
                            column.ReadOnly = true;
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        private int selectedEmployeeId;


        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();

                        string query = "DELETE FROM Employee WHERE Id = @Id";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@Id", selectedEmployeeId);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Dữ liệu đã xóa.");

                            ReloadData();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];
                guna2TextBox1.Text = row.Cells["Name"].Value.ToString();
                guna2TextBox2.Text = row.Cells["Age"].Value.ToString();
                if (DateTime.TryParse(row.Cells["CreatedAt"].Value.ToString(), out DateTime createdAt))
                {
                    guna2DateTimePicker1.Value = createdAt;
                }
                comboBox1.Text = row.Cells["Role"].Value.ToString();
                if (row.Cells["Avatar"].Value != DBNull.Value)
                {
                    byte[] imageData = (byte[])row.Cells["Avatar"].Value;
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        guna2PictureBox1.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    guna2PictureBox1.Image = null;
                }

                selectedEmployeeId = Convert.ToInt32(row.Cells["Id"].Value);
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {


            try
            {
                string connectionString = conStr;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE Employee SET Name = @Name, Age = @Age, Role = @Role, Avatar = @Avatar, CreatedAt = @CreatedAt WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Name", guna2TextBox1.Text);
                        cmd.Parameters.AddWithValue("@Age", guna2TextBox2.Text);
                        cmd.Parameters.AddWithValue("@Role", comboBox1.Text);
                        cmd.Parameters.AddWithValue("@Avatar", getImage());
                        cmd.Parameters.AddWithValue("@CreatedAt", guna2DateTimePicker1.Value);
                        cmd.Parameters.AddWithValue("@Id", selectedEmployeeId);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Dữ liệu đã được cập nhật.");
                        ReloadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();

                    string searchKeyword = guna2TextBox3.Text;

                    string query = "SELECT * FROM Employee WHERE Name LIKE @SearchKeyword";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@SearchKeyword", "%" + searchKeyword + "%");
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    guna2DataGridView1.DataSource = dataTable;
                    guna2DataGridView1.Columns["Avatar"].Visible = false;

                    guna2DataGridView1.Columns["Name"].HeaderText = "Tên";
                    guna2DataGridView1.Columns["Age"].HeaderText = "Tuổi";
                    guna2DataGridView1.Columns["CreatedAt"].HeaderText = "Ngày vào làm";
                    guna2DataGridView1.Columns["role"].HeaderText = "Chức vụ";

                    foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
                    {
                        column.ReadOnly = true;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }

        }

        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
            TableSalaryEmployee tableSalaryForm = new TableSalaryEmployee();
            tableSalaryForm.Show();
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            SalaryCalculation salaryCalculation = new SalaryCalculation();
            salaryCalculation.Show();
        }

        private async void guna2Button8_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    await connection.OpenAsync();

                    int page = int.Parse(guna2TextBox4.Text)+1; 

                    int rowsPerPage = 10;

                    int offset = (page - 1) * rowsPerPage;
                    string query = $"SELECT * FROM Employee ORDER BY ID OFFSET {offset} ROWS FETCH NEXT {rowsPerPage} ROWS ONLY";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    await Task.Run(() => adapter.Fill(dataTable));

                    guna2DataGridView1.Invoke((Action)(() =>
                    {
                        guna2DataGridView1.DataSource = dataTable;
                        guna2DataGridView1.Columns["Avatar"].Visible = false;

                        guna2DataGridView1.Columns["Name"].HeaderText = "Tên";
                        guna2DataGridView1.Columns["Age"].HeaderText = "Tuổi";
                        guna2DataGridView1.Columns["CreatedAt"].HeaderText = "Ngày vào làm";
                        guna2DataGridView1.Columns["role"].HeaderText = "Chức vụ";

                        foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
                        {
                            column.ReadOnly = true;
                        }
                    }));
                    guna2TextBox4.Text = page.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }

        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private async void guna2Button9_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    await connection.OpenAsync();

                    int page = int.Parse(guna2TextBox4.Text);
                    if (page == 1)
                    {
                        return;
                    }
                    else
                    {
                        page -= 1;
                    }

                    int rowsPerPage = 10;

                    int offset = (page - 1) * rowsPerPage;
                    string query = $"SELECT * FROM Employee ORDER BY ID OFFSET {offset} ROWS FETCH NEXT {rowsPerPage} ROWS ONLY";



                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    await Task.Run(() => adapter.Fill(dataTable));

                    guna2DataGridView1.Invoke((Action)(() =>
                    {
                        guna2DataGridView1.DataSource = dataTable;
                        guna2DataGridView1.Columns["Avatar"].Visible = false;

                        guna2DataGridView1.Columns["Name"].HeaderText = "Tên";
                        guna2DataGridView1.Columns["Age"].HeaderText = "Tuổi";
                        guna2DataGridView1.Columns["CreatedAt"].HeaderText = "Ngày vào làm";
                        guna2DataGridView1.Columns["role"].HeaderText = "Chức vụ";

                        foreach (DataGridViewColumn column in guna2DataGridView1.Columns)
                        {
                            column.ReadOnly = true;
                        }
                    }));
                    guna2TextBox4.Text = page.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
