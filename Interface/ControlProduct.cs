using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using DevExpress.Utils.Extensions;
using Guna.UI2.WinForms;
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
    public partial class ControlProduct : UserControl
    {
        public ControlProduct()
        {
            InitializeComponent();
        }
        List<GetAllWareHouseById_Result> l = new List<GetAllWareHouseById_Result>();
        HighlandEntities db = new HighlandEntities();
        public ControlProduct(int warehouse_id, List<GetAllWareHouseById_Result> l = null)
        {
            InitializeComponent();
            List<GetAllWareHouseById_Result> tmp = db.GetAllWareHouseById(warehouse_id).ToList();
            tmp.Reverse();
            tmp = tmp.FindAll(each => each.quantity > 0).ToList();
            if (l != null)
            {
                foreach (GetAllWareHouseById_Result i in l)
                {
                    if (tmp.Any(item => item.product_id == i.product_id))
                    {
                        tmp.RemoveAll(item => item.product_id == i.product_id);
                    }
                }
            }
            this.l = tmp;
            guna2ComboBox1.DataSource = tmp;
            guna2ComboBox1.DisplayMember = "product_name";
            guna2ComboBox1.ValueMember = "product_id";
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = guna2ComboBox1.SelectedItem as GetAllWareHouseById_Result;
            guna2NumericUpDown1.Value = item.quantity;
        }

        public GetAllWareHouseById_Result ResultP()
        {
            var i = guna2ComboBox1.SelectedItem as GetAllWareHouseById_Result;
            i.quantity = (int)guna2NumericUpDown1.Value;
            return i;
        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Control parentControl = this.Parent;

            if (parentControl is Panel)
            {
                Panel panel = (Panel)parentControl;
                panel.Controls.Remove(this);
            }
            int newY = 0;
            foreach (Control control in parentControl.Controls.OfType<ControlProduct>())
            {
                control.Location = new Point(control.Location.X, newY);
                newY += control.Height;
            }
        }
    }
}
