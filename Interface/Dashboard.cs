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
            //label1.Text = "T"+ DateTime.Now.Month;
        }


      
        private void Dashboard_Load(object sender, EventArgs e)
        {
            HighlandEntities db = new HighlandEntities();

            lbE.Text = db.Employees.Count().ToString();

            lbO.Text = db.OrderPDs.Count().ToString();

            lbT.Text = db.OrderPDs.Sum(o => o.Total).ToString();

            var list = db.Top5Product().ToList();

            gunaChart1.Legend.Position = Guna.Charts.WinForms.LegendPosition.Bottom;
            gunaChart1.XAxes.Display = false;
            gunaChart1.YAxes.Display = false;

            var dataset1 = new Guna.Charts.WinForms.GunaPieDataset();

            foreach(var i in list)
            {
                dataset1.DataPoints.Add(i.productname, (double)i.TotalQuantity);
            }
            gunaChart1.Datasets.Add(dataset1);
            gunaChart1.Update();


           
            Random r = new Random();
            for (int i = 1; i <= 5; i++)
            {
                chartA.Series["Series1"].Points.AddXY("chi nhánh " + i, r.Next(90, 999));
                chartB.Series["Series1"].Points.AddXY("tháng " + i, r.Next(90, 999));
                
            }

            if (chartB.Series["Series1"].Points.Count >= 2)
            {
                chartB.Series["Series1"].Points[0].Color = Color.FromArgb(82, 182, 154);
                chartB.Series["Series1"].Points[1].Color = Color.FromArgb(52, 160, 164);
                chartB.Series["Series1"].Points[2].Color = Color.FromArgb(22, 138, 173);
                chartB.Series["Series1"].Points[3].Color = Color.FromArgb(26, 117, 159);
                chartB.Series["Series1"].Points[4].Color = Color.FromArgb(30, 96, 145);
            }
        }

    
    }
}