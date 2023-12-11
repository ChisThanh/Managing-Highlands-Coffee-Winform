using Interface.Helpers;
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
    public partial class frmIngredient : Form
    {
        HighlandEntities db = new HighlandEntities();
        public frmIngredient()
        {
            InitializeComponent();
        }

        private void Ingredient_Load(object sender, EventArgs e)
        {
            var l = db.Ingredients.ToList();
            foreach (var item in l)
            {
                int rowIndex = guna2DataGridView1.Rows.Add();
                guna2DataGridView1.Rows[rowIndex].Cells[0].Value = item.product_id;
                guna2DataGridView1.Rows[rowIndex].Cells[1].Value = item.product_name;
                guna2DataGridView1.Rows[rowIndex].Cells[2].Value = item.Price;
                guna2DataGridView1.Rows[rowIndex].Cells[3].Value = Image.FromFile(@"..\..\images\icons8-delete-60.png");
                guna2DataGridView1.Rows[rowIndex].Cells[4].Value = Image.FromFile(@"..\..\images\Edit_1.png");
            }
        }

        private void load_GV(List<Ingredient> l)
        {
            guna2DataGridView1.Refresh();
            guna2DataGridView1.Rows.Clear();
            foreach (var item in l)
            {
                int rowIndex = guna2DataGridView1.Rows.Add();
                guna2DataGridView1.Rows[rowIndex].Cells[0].Value = item.product_id;
                guna2DataGridView1.Rows[rowIndex].Cells[1].Value = item.product_name;
                guna2DataGridView1.Rows[rowIndex].Cells[2].Value = item.Price;
                guna2DataGridView1.Rows[rowIndex].Cells[3].Value = Image.FromFile(@"..\..\images\icons8-delete-60.png");
                guna2DataGridView1.Rows[rowIndex].Cells[4].Value = Image.FromFile(@"..\..\images\Edit_1.png");
            }
        }
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = guna2DataGridView1.Rows[e.RowIndex];

            object ma = selectedRow.Cells[0].Value;
            var tmp = db.Ingredients.ToList().FirstOrDefault(each => each.product_id == (int)ma);
            if (e.ColumnIndex == 3)
            {
                if (MessageBox.Show("Bạn có muốn xóa", "Thông báo",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
               

                if (ma != null)
                {
                    try
                    {
                        db.Ingredients.Remove(tmp);
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Nguyên liệu không thể xóa. vì đang hoạt động");
                        return;
                    }

                    guna2DataGridView1.Rows.Remove(selectedRow);
                }
            }

            if (e.ColumnIndex == 4)
            {
                IngredientEdit f = new IngredientEdit(tmp);
                f.Show();
                var l = db.Ingredients.ToList();
                load_GV(l);
            }
        }

        private void guna2TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            string str = guna2TextBox1.Text;
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(str))
            {
                var tmp = db.Ingredients.ToList().FindAll(each => HP.ContainsWord(each.product_name, str) == true).OrderByDescending(each => each.product_id).ToList();
                load_GV(tmp);
                guna2Button1.Visible = true;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var l = db.Ingredients.ToList();
            load_GV(l);
            guna2Button1.Visible = false;
        }
    }
}
