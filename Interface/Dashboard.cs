using Interface.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;

namespace Interface
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            label1.Text = "T"+ DateTime.Now.Month;
            label26.Text = "Tháng " + DateTime.Now.Month;
        }


      
        private void Dashboard_Load(object sender, EventArgs e)
        {
            HighlandEntities db = new HighlandEntities();

            lbE.Text = db.Employees.Count().ToString();
             
            lbO.Text= db.OrderPDs.Count().ToString();

            lbT.Text = db.OrderPDs.Sum(o => o.Total).ToString();

  

            var list = db.Top5Product().ToList();
           








            string[] months = { "Sản phẩm A", "Sản phẩm B", "Sản phẩm C", "Sản phẩm D" };
            Random r = new Random();
            gunaChart1.Legend.Position = Guna.Charts.WinForms.LegendPosition.Right;
            gunaChart1.XAxes.Display = false;
            gunaChart1.YAxes.Display = false;

            var dataset1 = new Guna.Charts.WinForms.GunaPieDataset();

            foreach(var i in list)
            {
                dataset1.DataPoints.Add(i.productname, (double)i.TotalQuantity);
            }
            gunaChart1.Datasets.Add(dataset1);
            gunaChart1.Update();
           
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}