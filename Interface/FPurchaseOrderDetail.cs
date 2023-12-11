using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DataPlayer;
using Interface.Helpers;
using OfficeOpenXml;
using static OfficeOpenXml.ExcelErrorValue;

namespace Interface
{
    public partial class FPurchaseOrderDetail : Form
    {
        string order_id;
        List<Tuple<string, int, double>> _l = new List<Tuple<string, int, double>>();
        public FPurchaseOrderDetail()
        {
            InitializeComponent();
        }

        public FPurchaseOrderDetail(string order_id)
        {
            InitializeComponent();
            this.order_id = order_id;
        }
        private async void FPurchaseOrderDetail_Load(object sender, EventArgs e)
        {
            PurchaseOrderDetail p = new PurchaseOrderDetail();

            var list = await p.GetProductDetails(order_id);
            _l = list;
            if (list == null || list.Count <= 0)
            {
                this.Visible = true;
                MessageBox.Show("Không có sản phẩm nào!");
                this.Close();
            }
            int sum = 0;
            foreach ( var item in list )
            {
                int rowIndex = guna2DataGridView1.Rows.Add();
                guna2DataGridView1.Rows[rowIndex].Cells["Column1"].Value = item.Item1;
                guna2DataGridView1.Rows[rowIndex].Cells["Column2"].Value = item.Item2;
                guna2DataGridView1.Rows[rowIndex].Cells["Column3"].Value = item.Item3;
                sum += item.Item2 * (int)item.Item3;
            }

            textBox1.Text = FormatCurrency.FormatAmount(sum);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                var folderPath = folderDialog.SelectedPath;


            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string fileName = "PurchaseOrderDetail.xlsx";
            string filePath = Path.Combine(folderPath, fileName);

            // Tạo một tệp Excel mới
            bool fileExists = File.Exists(filePath);
            FileInfo newFile = new FileInfo(filePath);

                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    int rowStart = 2;
                    int colStart = 1;
                    worksheet.Cells[1, 1].Value = "Trên sản phẩm";
                    worksheet.Cells[1, 2].Value = "Số lượng";
                    worksheet.Cells[1, 3].Value = "Giá";



                    foreach (var item in _l)
                    {
                        worksheet.Cells[rowStart, colStart].Value = item.Item1;
                        worksheet.Cells[rowStart, colStart + 1].Value = item.Item2;
                        worksheet.Cells[rowStart, colStart + 2].Value = item.Item3;
                        rowStart++;
                    }

                    worksheet.Cells.AutoFitColumns();
                    package.Save();
                }

                MessageBox.Show("Tạo tệp Excel thành công.");

            }
        }
    }
}
