using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace Interface
{
    public partial class ProductTransfers : Form
    {
        public ProductTransfers()
        {
            InitializeComponent();
        }
        int x = 52;
        int y = 85;
        int controlCounter = 0;

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            ControlProduct a = new ControlProduct();

            a.Location = new Point(x, y + (controlCounter * 70)); 
            a.Size = new Size(700, 75);
            panel1.Controls.Add(a);

            controlCounter++; // Tăng biến đếm
        }

       
    }
}
