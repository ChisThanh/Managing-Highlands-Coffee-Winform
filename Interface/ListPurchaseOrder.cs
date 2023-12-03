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
        List<Tuple<int, string, DateTime, int>> list = new List<Tuple<int, string, DateTime, int>>();

        public  ListPurchaseOrder()
        {
            InitializeComponent();

           

        }

        private async void ListPurchaseOrder_Load(object sender, EventArgs e)
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            var purchaseOrders = await purchaseOrder.GetPurchaseOrdersWithSupplierName();
            list = purchaseOrders;
             DataGirdView(purchaseOrders);
        }
        public void DataGirdView(List<Tuple<int, string, DateTime, int>> list)
        {
            guna2DataGridView1.Rows.Clear();
            guna2DataGridView1.Refresh();
            foreach (var order in list)
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

        private  void guna2TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            string str = guna2TextBox1.Text;
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(str))
            {
                var tmp = list.FindAll(each => ContainsWord(each.Item2, str) == true).OrderByDescending(each => each.Item3).ToList();
                 DataGirdView(tmp);
                guna2Button1.Visible = true;
            }
        }

        public bool ContainsWord(string input, string searchTerm)
        {
            StringComparison comparison =  StringComparison.CurrentCultureIgnoreCase;

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] searchTermBytes = Encoding.UTF8.GetBytes(searchTerm);

            string utf8Input = Encoding.UTF8.GetString(inputBytes);
            string utf8SearchTerm = Encoding.UTF8.GetString(searchTermBytes);

            return utf8Input.IndexOf(utf8SearchTerm, comparison) >= 0;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DataGirdView(list);
            guna2Button1.Visible=false;
            guna2TextBox1.Clear();
        }
    }
}
