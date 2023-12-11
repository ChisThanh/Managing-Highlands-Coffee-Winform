using Interface.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Interface.Helpers;
using System.Windows.Media;
using Color = System.Drawing.Color;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Collections.Generic;
using Guna.UI2.WinForms;

namespace Interface
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }
        HighlandEntities db = new HighlandEntities();

        string type = "year";
        int? value = DateTime.Now.Year;
        DateTime ?date = null;



        private void Dashboard_Load(object sender, EventArgs e)
        {
            gunaChart1.Legend.Position = Guna.Charts.WinForms.LegendPosition.Bottom;
            gunaChart1.XAxes.Display = false;
            gunaChart1.YAxes.Display = false;
            guna2DateTimePicker1.Visible = true;
            int n = db.NumberOfEmployee().First() ?? 0;
            lbE.Text = n.ToString();

            LoadData();

            var listTOP5 = db.TOP5MONTH();
            foreach (var item in listTOP5)
                chartB.Series["Series1"].Points.AddXY(HP.GetLast2Words("T " + item.DayChart), item.TotalAmount);

            if (chartB.Series["Series1"].Points.Count >= 2)
            {
                chartB.Series["Series1"].Points[0].Color = Color.FromArgb(82, 182, 154);
                chartB.Series["Series1"].Points[1].Color = Color.FromArgb(52, 160, 164);
                chartB.Series["Series1"].Points[2].Color = Color.FromArgb(22, 138, 173);
                chartB.Series["Series1"].Points[3].Color = Color.FromArgb(26, 117, 159);
                //chartB.Series["Series1"].Points[4].Color = Color.FromArgb(30, 96, 145);
            }
            for (int i = (int)DateTime.Now.Year; i >= 2010; i--)
            {
                guna2ComboBox2.Items.Add(i);
            }
            guna2ComboBox2.SelectedIndex = 0;

        }

        private void LoadData()
        {
            var tonkho = db.Database.SqlQuery<double>("SELECT dbo.sltonkho()").FirstOrDefault();
            var Quantity = db.FilterOrdersQuantity(type, value, date).First().ToString() ?? "0";
            var Total = db.FilterOrdersTotal(type, value, date).First() ?? 0;
            label7.Text = tonkho.ToString();
            if (Quantity == "0")
            {
                MessageBox.Show("Doanh số trống");
                return;
            }
            lbO.Text = Quantity;

            lbT.Text = FormatCurrency.FormatAmount((int)Total);

            gunaChart1.DataBindings.Clear();
            gunaChart1.Datasets.Clear();
            gunaChart1.Update();

            var list = db.Top5Product(type, value, date).ToList();

            var dataset1 = new Guna.Charts.WinForms.GunaPieDataset();
            foreach (var i in list)
            {
                dataset1.DataPoints.Add(i.productname, (double)i.TotalQuantity);
            }
            gunaChart1.Datasets.Add(dataset1);
            gunaChart1.Update();


            chartA.Series["Series1"].Points.Clear();
            var listTOP10 = db.TOP10BRANCH(type, value, date);
            foreach (var item in listTOP10)
                chartA.Series["Series1"].Points.AddXY(HP.GetLast2Words(item.Name), item.TotalAmount);

        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            ComboBox comboBox = (ComboBox)sender;
            int selectedIndex = comboBox.SelectedIndex;
            guna2ComboBox2.Items.Clear();
            switch (selectedIndex)
            {
                case 0:
                    guna2DateTimePicker1.Visible = true;
                    guna2ComboBox2.Visible = false;
                    break;
                case 1:
                    guna2DateTimePicker1.Visible = false;
                    for (int i = DateTime.Now.Month; i >= 1; i--)
                    {
                        guna2ComboBox2.Items.Add(i);
                    }
                    guna2ComboBox2.SelectedIndex = 0;
                    guna2ComboBox2.Visible = true;
                    break;
                case 2:
                    guna2DateTimePicker1.Visible = false;
                    for (int i = (int)DateTime.Now.Year; i >= 2010; i--)
                    {
                        guna2ComboBox2.Items.Add(i);
                    }
                    guna2ComboBox2.SelectedIndex = 0;
                    guna2ComboBox2.Visible = true;
                    break;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            int selectedIndex = guna2ComboBox1.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    type = "day";
                    date = guna2DateTimePicker1.Value;
                    value = null;
                    break;
                case 1:
                    type = "month";
                    date = null;
                    value = int.Parse(guna2ComboBox2.Text);
                 
                    break;
                case 2:
                    type = "year";
                    date = null;
                    value = int.Parse(guna2ComboBox2.Text);
                    break;
               

            }
            LoadData();
        }
        private void ExportListToExcel( string folderPath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string fileName = "Data.xlsx";
            string filePath = Path.Combine(folderPath, fileName);
            // Tạo một tệp Excel mới
            bool fileExists = File.Exists(filePath);
            FileInfo newFile = new FileInfo(filePath);

            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                int rowStart = 2;
                int colStart = 1;
                worksheet.Cells[1, 1].Value = "Trên chi nhánh"; 
                worksheet.Cells[1, 2].Value = "Tổng tiền";
   
                var t10br = db.TOP10BRANCH(type, value, date).ToList();
                var list = db.Top5Product(type, value, date).ToList();
                var t10m = db.TOP5MONTH().ToList();

                foreach (var item in t10br)
                { 
                    worksheet.Cells[rowStart, colStart].Value = item.Name;
                    worksheet.Cells[rowStart, colStart + 1].Value = item.TotalAmount;
                    rowStart++;

                }
                colStart = 4;
                rowStart = 1;
                worksheet.Cells[rowStart, colStart ].Value = "Tên sản phẩm";
                worksheet.Cells[rowStart, colStart + 1].Value = "Tổng số lượng";
                rowStart++;
                foreach (var item in list)
                {
                    worksheet.Cells[rowStart, colStart ].Value = item.productname;
                    worksheet.Cells[rowStart, colStart + 1].Value = item.TotalQuantity;
                    rowStart++;

                }
                rowStart = 1;
                colStart = 8;
                worksheet.Cells[rowStart, colStart].Value = "Tháng";
                worksheet.Cells[rowStart, colStart + 1].Value = "Tổng số lượng";
                rowStart++;

                foreach (var item in t10m)
                {
                    worksheet.Cells[rowStart, colStart].Value = item.DayChart;
                    worksheet.Cells[rowStart, colStart + 1].Value = item.TotalAmount;
                    rowStart++;

                }

                worksheet.Cells.AutoFitColumns();
                package.Save();
            }
        }
       

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                var folderPath = folderDialog.SelectedPath;
               
                ExportListToExcel( folderPath);
                MessageBox.Show("Tạo tệp Excel thành công.");
            }
        }

       
    }
}