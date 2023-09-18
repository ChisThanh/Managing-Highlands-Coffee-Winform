using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interface
{
    public partial class CreateCart : Form
    {

        
        public CreateCart()
        {
            InitializeComponent();
            
        }
        public CreateCart(string typeBtn)
        {
            InitializeComponent();
            btnSubmit.Text = typeBtn;
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
