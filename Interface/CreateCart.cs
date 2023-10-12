using System;
using System.Windows.Forms;
using DataPlayer;

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
                guna2TextBox3.Text = Op.Price.ToString();
                guna2TextBox2.Text = Op.Quantity.ToString();
            }

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

            Product tmp = new Product
            {
                ProductId = id,
                ProductName = guna2TextBox1.Text,
                Price = double.Parse(guna2TextBox3.Text),
                Quantity = int.Parse(guna2TextBox2.Text),
            };
            NewProduct = tmp;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
