using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataPlayer;

namespace Interface
{
    public partial class CreateSuppliers : Form
    {
        public CreateSuppliers()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Supplier supplier = new Supplier();
            string name = guna2TextBox1.Text;
            string email = guna2TextBox2.Text;
            string phone = guna2TextBox3.Text;
            if(supplier.InsertSupplier(name, email, phone))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
