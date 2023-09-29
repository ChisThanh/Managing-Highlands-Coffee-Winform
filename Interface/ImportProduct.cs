using Guna.UI2.WinForms;
using Interface.Models;
using OfficeOpenXml;
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
using Interface.Helpers;
using System.Collections.ObjectModel;

namespace Interface
{
    public partial class ImportProduct : Form
    {
        public List<Product> products = new List<Product>();
        public ImportProduct()
        {
            InitializeComponent();
            //Excel excel = new Excel();
            //products = excel.FileEXProduct("D:\\Code\\Interface\\Excel\\Book1.xlsx");
            dataGirdView();
        }

        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var f = new CreateCart("Thêm");
            if (f.ShowDialog() == DialogResult.OK)
            {
                Product newProduct = f.NewProduct;
                products.Add(newProduct);
                addRow(newProduct);
            }
        }

      
        private void guna2Button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Chọn tệp tin";
            openFileDialog.Filter = "Tất cả các tệp tin|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                Excel excel = new Excel();
                products = excel.FileEXProduct(selectedFilePath);
                dataGirdView();
            }
        }
       

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];

                string columnName = guna2DataGridView1.Columns[e.ColumnIndex].Name;

                if (columnName == "Column5")
                {

                    int productId = Convert.ToInt32(selectedRow.Cells["Column1"].Value);
                    string productName = selectedRow.Cells["Column2"].Value.ToString();
                    double price = Convert.ToDouble(selectedRow.Cells["Column3"].Value);
                    int stockQuantity = Convert.ToInt32(selectedRow.Cells["Column4"].Value);


                    var f = new CreateCart("Sửa",
                        (new Product
                        {
                            ProductId = productId,
                            ProductName = productName,
                            Price = price,
                            StockQuantity = stockQuantity,
                        }));

                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        Product newProduct = f.NewProduct;

                        Product productToEdit = products.FirstOrDefault(p => p.ProductId == newProduct.ProductId);

                        if (productToEdit != null)
                        {
                            // Sửa thông tin sản phẩm
                            productToEdit.ProductName = newProduct.ProductName;
                            productToEdit.Price = newProduct.Price;
                            productToEdit.StockQuantity = newProduct.StockQuantity;

                            MessageBox.Show("Sản phẩm đã được sửa thành công!");
                            dataGirdView();
                        }
                        else
                        {
                            MessageBox.Show("Lỗi");
                        }
                       
                    }

                   
                }
                else if (columnName == "Column6")
                {
                    DialogResult d = MessageBox.Show("Bạn có chắc chắn muốn xóa","Cảnh báo",MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
                    if (d == DialogResult.OK)
                    {
                        guna2DataGridView1.Rows.Remove(selectedRow);
                    }
                }
            }
        }

        private void addRow(Product p)
        {
            int rowIndex = guna2DataGridView1.Rows.Add();
            guna2DataGridView1.Rows[rowIndex].Cells["Column1"].Value = p.ProductId;
            guna2DataGridView1.Rows[rowIndex].Cells["Column2"].Value = p.ProductName;
            guna2DataGridView1.Rows[rowIndex].Cells["Column3"].Value = p.Price;
            guna2DataGridView1.Rows[rowIndex].Cells["Column4"].Value = p.StockQuantity;
            guna2DataGridView1.Rows[rowIndex].Cells["Column5"].Value = Image.FromFile(@"..\..\images\icons8-edit-50.png");
            guna2DataGridView1.Rows[rowIndex].Cells["Column6"].Value = Image.FromFile(@"..\..\images\icons8-delete-60.png");
        }
        private void dataGirdView()
        {
            guna2DataGridView1.DataSource = null;
            guna2DataGridView1.Rows.Clear();
            foreach (var product in products)
            {
                addRow(product);
            }

        }
    }
    
}

