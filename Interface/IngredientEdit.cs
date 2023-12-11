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
    public partial class IngredientEdit : Form
    {
        HighlandEntities db = new HighlandEntities();
        Ingredient _i = new Ingredient();
        
        public IngredientEdit(Ingredient i)
        {
            InitializeComponent();
            this._i = i;
            textBox1.Text = i.product_name;
            textBox2.Text = i.Price.ToString();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var tmp = db.Ingredients.FirstOrDefault(each => each.product_id == _i.product_id);
            tmp.product_name = textBox1.Text;
            tmp.Price = double.Parse(textBox2.Text);
            db.SaveChanges();
            this.Close();
        }
    }
}
