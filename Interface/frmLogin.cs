using Interface;
using Interface.Models;
using System;
using System.Linq;
using System.Windows.Forms;

namespace QlBanHang
{
    public partial class frmLogin : Form
    {
        HighlandEntities db = new HighlandEntities();
        public frmLogin()
        {
            InitializeComponent();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string ten = txtDN.Text;
            string pas = txtMK.Text;
            var lg = db.Logins.ToList().FirstOrDefault(each => each.TenTk.Trim() == ten.Trim() && each.Mk.Trim() == pas.Trim());
            if(lg == null)
            {
                MessageBox.Show("Tài khoảng không tồn tại"); 
                return;
            }
            if(lg.QuyenTC == 3)
            {
                Main main = new Main();
                main.Show();
            }
            else
            {
                MessageBox.Show("Nhân viên không có quyền"); 
            }

        }

        private void chkPass_CheckedChanged(object sender, EventArgs e)
        {
            txtMK.PasswordChar = chkPass.Checked ? '\0' : '*';
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}



