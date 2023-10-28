using Interface.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Interface.Helpers;

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
            HighlandEntities db = new HighlandEntities();

            lbE.Text = db.Employees.Count().ToString();

            lbO.Text = db.OrderPDs.Count().ToString();

            lbT.Text = FormatCurrency.FormatAmount((int)db.OrderPDs.Sum(o => o.Total));

            var list = db.Top5Product("year", DateTime.Now.Year, null).ToList();

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


            var listTOP10 = db.TOP10BRANCH("year", DateTime.Now.Year, null);
            foreach (var item in listTOP10)
            {
                chartA.Series["Series1"].Points.AddXY(HP.GetLast2Words(item.Name), item.TotalAmount);
            }

            var listTOP5 = db.TOP5MONTH();
            foreach (var item in listTOP5)
            {
                chartB.Series["Series1"].Points.AddXY(HP.GetLast2Words("T "+item.DayChart), item.TotalAmount);
            }


            if (chartB.Series["Series1"].Points.Count >= 2)
            {
                chartB.Series["Series1"].Points[0].Color = Color.FromArgb(82, 182, 154);
                chartB.Series["Series1"].Points[1].Color = Color.FromArgb(52, 160, 164);
                chartB.Series["Series1"].Points[2].Color = Color.FromArgb(22, 138, 173);
                chartB.Series["Series1"].Points[3].Color = Color.FromArgb(26, 117, 159);
                chartB.Series["Series1"].Points[4].Color = Color.FromArgb(30, 96, 145);
            }

            for (int i = DateTime.Now.Day; i >= 1; i--)
            {
                guna2ComboBox2.Items.Add(i);
            }
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            int selectedIndex = comboBox.SelectedIndex;
            guna2ComboBox2.Items.Clear();
            switch (selectedIndex)
            {
                case 0:
                    for (int i = DateTime.Now.Day; i >= 1; i--)
                    {
                        guna2ComboBox2.Items.Add(i);
                    }
                    break;
                case 1:
                    for (int i = DateTime.Now.Month; i >= 1; i--)
                    {
                        guna2ComboBox2.Items.Add(i);
                    }
                    break;
                case 2:
                    for (int i = (int)DateTime.Now.Year; i >= 2010; i--)
                    {
                        guna2ComboBox2.Items.Add(i);
                    }
                    break;
                default:
                    break;

            }
            guna2ComboBox2.SelectedIndex = 0;


        }
    }
}