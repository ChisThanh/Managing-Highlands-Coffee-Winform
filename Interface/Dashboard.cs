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

namespace Interface
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }


      
        private void Dashboard_Load(object sender, EventArgs e)
        {
            string[] months = { "January", "February", "March", "April" };
            Random r = new Random();
            gunaChart1.Legend.Position = Guna.Charts.WinForms.LegendPosition.Right;
            gunaChart1.XAxes.Display = false;
            gunaChart1.YAxes.Display = false;

            var dataset1 = new Guna.Charts.WinForms.GunaPieDataset();

            for (int i = 0; i < months.Length; i++)
            {
                int num = r.Next(10, 100);
                dataset1.DataPoints.Add(months[i], num);
            }

            gunaChart1.Datasets.Add(dataset1);
            gunaChart1.Update();
        }
    }
}