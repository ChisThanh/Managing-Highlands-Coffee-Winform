using Guna.Charts;
using System;
using System.Windows.Forms;
namespace Interface
{
    public partial class Main : Form
    {
        private Form activeForm = null;

        public Main()
        {
            InitializeComponent();
            customSizePanel();
            openChildForm(new Dashboard());
        }
        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
            this.Close();
        }
        private void customSizePanel()
        {
            panel2.Visible = false;
            panel3.Visible = false;
        }
        private void showSubMenu(Panel subMenu)
        {
            if (!subMenu.Visible)
                subMenu.Visible = true;
            else
                subMenu.Visible = false;
        }
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
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

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            showSubMenu(panel3);
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            showSubMenu(panel4);
        }
        private void guna2Button5_Click(object sender, EventArgs e)
        {
            showSubMenu(panel2);
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            openChildForm(new Dashboard());
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            openChildForm(new ImportPurchaseOrder());
        }
        private void guna2Button8_Click(object sender, EventArgs e)
        {
            openChildForm(new Employee());
        }
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            openChildForm(new ListPurchaseOrder());
        }
        private void guna2Button10_Click(object sender, EventArgs e)
        {
            openChildForm(new Employee());
        }
        private void guna2Button10_Click_1(object sender, EventArgs e)
        {
            openChildForm(new ImportPurchaseOrder());
        }
        private void guna2Button11_Click(object sender, EventArgs e)
        {
            openChildForm(new ListPurchaseOrder());
        }
        private void guna2Button6_Click(object sender, EventArgs e)
        {
            openChildForm(new ListWarehouses());
        }
        private void guna2Button1_Click_2(object sender, EventArgs e)
        {
            openChildForm(new frmProductTransfers());
        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            openChildForm(new frmIngredient());
        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            openChildForm(new ProductTransfers());
        }
    }
}