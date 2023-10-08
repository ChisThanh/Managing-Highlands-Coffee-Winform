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
    public partial class FWarehousesDetail : Form
    {
        string id;
        public FWarehousesDetail()
        {
            InitializeComponent();
        }

        public FWarehousesDetail(string id)
        {
            InitializeComponent();
            this.id = id;
        }
        private async void FPurchaseOrderDetail_Load(object sender, EventArgs e)
        {
            Warehouse w = new Warehouse();

            var list = await w.GetProductInWarehouse();

            if (list == null || list.Count <= 0)
            {
                this.Visible = true;
                MessageBox.Show("Không có sản phẩm nào!");
                this.Close();
            }

            foreach ( var item in list )
            {
                int rowIndex = guna2DataGridView1.Rows.Add();
                guna2DataGridView1.Rows[rowIndex].Cells["Column1"].Value = item.Item1;
                guna2DataGridView1.Rows[rowIndex].Cells["Column2"].Value = item.Item2;
            }
        }
    }
}
