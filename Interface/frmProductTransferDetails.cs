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
    public partial class frmProductTransferDetails : Form
    {
        HighlandEntities db = new HighlandEntities();
        int id = 0;
        public frmProductTransferDetails(int id)
        {
            InitializeComponent();
            this.id = id;
        }
        private void load_GV(List<ProductTransferDetail> l)
        {
            guna2DataGridView1.Refresh();
            guna2DataGridView1.Rows.Clear();
            var br = db.Ingredients.ToList();
            foreach (var item in l)
            {
                var tmp = br.FirstOrDefault(each => each.product_id == item.product_id);
                int rowIndex = guna2DataGridView1.Rows.Add();
                guna2DataGridView1.Rows[rowIndex].Cells[0].Value = tmp.product_name;
                guna2DataGridView1.Rows[rowIndex].Cells[1].Value = item.quantity;
            }
        }

        private void frmProductTransferDetails_Load(object sender, EventArgs e)
        {
            var l = db.ProductTransferDetails.ToList().FindAll(each =>each.transfer_id == id);
            load_GV(l);
        }
    }
}
