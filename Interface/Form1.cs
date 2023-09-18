using Guna.UI2.WinForms;
using Interface.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interface
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            readFile();
        }

        public void showForm(string typeBtn)
        {
            var f = new CreateCart(typeBtn);
            f.Show();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            showForm("Thêm");
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            showForm("Sửa");
        }
        public void readFile()
        {
            List<Supplier> suppliers = new List<Supplier>();
            string duongDanTep = "D:\\Code\\Interface\\Interface\\data-2.txt";
            try
            {
                if (File.Exists(duongDanTep))
                {
                    // Đọc dữ liệu từ tệp văn bản
                    string[] lines = File.ReadAllLines(duongDanTep);

                    // Tạo DataTable để chứa dữ liệu
                    DataTable dt = new DataTable();

                    // Tạo cột cho DataGridView
                    string[] columns = lines[0].Split('\t');
                    foreach (string column in columns)
                    {
                        dt.Columns.Add(column);
                    }

                    // Thêm dữ liệu từ tệp vào DataTable (bỏ qua dòng đầu tiên vì nó chứa tiêu đề cột)
                    for (int i = 1; i < lines.Length; i++)
                    {

                        string[] data = lines[i].Split('\t');
                        Supplier s = new Supplier();
                        s.ID = data[0];
                        s.PID = data[1];
                        s.Name = data[2];
                        s.donGia = int.Parse(data[4]);
                        suppliers.Add(s);
                        dt.Rows.Add(data);
                    }



                    // Hiển thị dữ liệu trong DataGridView
                    DataGV.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("Tệp tin không tồn tại.", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi");
            }

        }
    }
}
