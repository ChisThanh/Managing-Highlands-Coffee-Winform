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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            customSizePanel();
            //ApplicationDbContext db = new ApplicationDbContext();
            //db.getAllTable("product");
        }
        private void customSizePanel()
        {
            panel1.Visible = false;
            guna2Panel2.Visible = false;
            panel2.Visible = false;
        }
        private void hideSubMenu()
        {
            if (panel1.Visible)
                panel1.Visible = false;
            if (guna2Panel2.Visible)
                guna2Panel2.Visible = false;
            if (panel2.Visible)
                panel2.Visible = false;
        }
        private void showSubMenu(Panel subMenu)
        {
            if (!subMenu.Visible)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;
        }

        private Form activeForm = null;
        private void openChildForm(Form childForm)
        {
            if(activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelChildForm.Controls.Add(childForm);
            panelChildForm.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            showSubMenu(panel1);
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            showSubMenu(guna2Panel2);

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            openChildForm(new ImportProduct());
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            showSubMenu(panel2);

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            openChildForm(new ListPurchaseOrder());
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            openChildForm(new Employee());
        }

        private void guna2Button13_Click(object sender, EventArgs e)
        {
            openChildForm(new ListWarehouses());

        }
    }
}