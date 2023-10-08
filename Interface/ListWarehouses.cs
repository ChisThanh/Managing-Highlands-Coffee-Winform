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
    public partial class ListWarehouses : Form
    {
        public ListWarehouses()
        {
            InitializeComponent();
        }

        private async void ListPurchaseOrder_Load(object sender, EventArgs e)
        {
            await DataGirdView();
        }
        public async Task DataGirdView()
        {
            Warehouse wareHouse = new Warehouse();
            var wareHouses = await wareHouse.GetAll();
            foreach (var i in wareHouses)
            {
               
                int rowIndex = guna2DataGridView1.Rows.Add();
                guna2DataGridView1.Rows[rowIndex].Cells["Column1"].Value = i.WarehousesId;
                guna2DataGridView1.Rows[rowIndex].Cells["Column2"].Value = i.WarehousesName;
                guna2DataGridView1.Rows[rowIndex].Cells["Column5"].Value = i.Location;
                guna2DataGridView1.Rows[rowIndex].Cells["Column4"].Value = Image.FromFile(@"..\..\images\icons8-view-60.png");
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell selectedCell = guna2DataGridView1.Rows[e.RowIndex].Cells[0];
                string id = selectedCell.Value.ToString();

                FWarehousesDetail f = new FWarehousesDetail(id);
                f.Show();
            }
        }
    }
}
