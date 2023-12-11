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
    public partial class frmProductTransfers : Form
    {
        HighlandEntities db = new HighlandEntities();
        public frmProductTransfers()
        {
            InitializeComponent();
        }
        private void load_GV(List<ProductTransfer> l)
        {
            guna2DataGridView1.Refresh();
            guna2DataGridView1.Rows.Clear();
            var br = db.Branches.ToList();
            foreach (var item in l)
            {
                var tmpfrom = br.FirstOrDefault(each => each.Id == item.from_warehouse_id);
                var tmpto = br.FirstOrDefault(each => each.Id == item.to_warehouse_id);
                int rowIndex = guna2DataGridView1.Rows.Add();
                guna2DataGridView1.Rows[rowIndex].Cells[0].Value = item.transfer_id;
                guna2DataGridView1.Rows[rowIndex].Cells[1].Value = tmpfrom.Name;
                guna2DataGridView1.Rows[rowIndex].Cells[2].Value = tmpto.Name;
                guna2DataGridView1.Rows[rowIndex].Cells[3].Value = item.transfer_date;
                guna2DataGridView1.Rows[rowIndex].Cells[4].Value = Image.FromFile(@"..\..\images\icons8-view-60.png");
            }
        }

        private void frmProductTransfers_Load(object sender, EventArgs e)
        {
            var l = db.ProductTransfers.ToList();
            load_GV(l);
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = guna2DataGridView1.Rows[e.RowIndex];

            object ma = selectedRow.Cells[0].Value;
            var tmp = db.Ingredients.ToList().FirstOrDefault(each => each.product_id == (int)ma);
            if (e.ColumnIndex == 4)
            {
                frmProductTransferDetails f = new frmProductTransferDetails((int)ma);
                f.Show();
            }
        }
    }
}
