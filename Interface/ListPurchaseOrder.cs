using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPlayer;
using System.Windows.Forms;
using Interface.Helpers;

namespace Interface
{
    public partial class ListPurchaseOrder : Form
    {
        public ListPurchaseOrder()
        {
            InitializeComponent();
        }

        private async void ListPurchaseOrder_Load(object sender, EventArgs e)
        {
            await DataGirdView();
        }
        public async Task DataGirdView()
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            var purchaseOrders = await purchaseOrder.GetPurchaseOrdersWithSupplierName();
            foreach (var order in purchaseOrders)
            {
                int orderId = order.Item1;
                string supplierName = order.Item2;
                DateTime orderDate = order.Item3;
                int total = order.Item4;
                int rowIndex = guna2DataGridView1.Rows.Add();
                guna2DataGridView1.Rows[rowIndex].Cells["Column1"].Value = orderId;
                guna2DataGridView1.Rows[rowIndex].Cells["Column2"].Value = supplierName;
                guna2DataGridView1.Rows[rowIndex].Cells["Column3"].Value = orderDate;
                guna2DataGridView1.Rows[rowIndex].Cells["Column4"].Value = Image.FromFile(@"..\..\images\icons8-view-60.png");
                guna2DataGridView1.Rows[rowIndex].Cells["Column5"].Value = FormatCurrency.FormatAmount(total);
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell selectedCell = guna2DataGridView1.Rows[e.RowIndex].Cells[0];
                string order_id = selectedCell.Value.ToString();

                FPurchaseOrderDetail f = new FPurchaseOrderDetail(order_id);
                f.Show();
            }
        }
    }
}
