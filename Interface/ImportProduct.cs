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

namespace Interface
{
    public partial class ImportProduct : Form
    {
        public ImportProduct()
        {
            InitializeComponent();
            Excel excel = new Excel();
            DataGV.DataSource = excel.FileEXProduct();
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

      
    }
}

