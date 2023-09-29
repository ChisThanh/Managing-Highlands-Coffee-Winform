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

namespace Interface
{
    public partial class CreateCart : Form
    {

        public Product NewProduct { get; private set; }
        private int id = 9999;
        public CreateCart()
        {
            InitializeComponent();
        }
        public CreateCart(string typeBtn, Product Op = null)
        {
            InitializeComponent();
            btnSubmit.Text = typeBtn;
            if (Op != null)
            {
                this.id = Op.ProductId;
                guna2TextBox1.Text = Op.ProductName;
                guna2TextBox4.Text = Op.Description;
                guna2TextBox3.Text = Op.Price.ToString();
                guna2TextBox2.Text = Op.StockQuantity.ToString();
            }

        }




        private void btnSubmit_Click(object sender, EventArgs e)
        {

            Product tmp = new Product
            {
                ProductId = id,
                ProductName = guna2TextBox1.Text,
                Description = guna2TextBox4.Text,
                Price = double.Parse(guna2TextBox3.Text),
                StockQuantity = int.Parse(guna2TextBox2.Text),
                MainWarehouse = 0
            };
            NewProduct = tmp;
            this.DialogResult = DialogResult.OK;
            this.Close();

        }
    }
}
