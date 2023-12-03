using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataPlayer;
using Guna.UI2.WinForms;

namespace Interface
{
    public partial class ListWarehouses : Form
    {
        List<Warehouse> list = new List<Warehouse>();
        public ListWarehouses()
        {
            InitializeComponent();
        }

        private async void ListPurchaseOrder_Load(object sender, EventArgs e)
        {
            Warehouse wareHouse = new Warehouse();
            list = await wareHouse.GetAll();
             DataGirdView(list);
        }
        public  void DataGirdView(List<Warehouse> list)
        {
            guna2DataGridView1.Rows.Clear();
            guna2DataGridView1.Refresh();
            foreach (var i in list)
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

     

        public bool ContainsWord(string input, string searchTerm)
        {
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase;

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] searchTermBytes = Encoding.UTF8.GetBytes(searchTerm);

            string utf8Input = Encoding.UTF8.GetString(inputBytes);
            string utf8SearchTerm = Encoding.UTF8.GetString(searchTermBytes);

            return utf8Input.IndexOf(utf8SearchTerm, comparison) >= 0;
        }

      

        private void guna2TextBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            string str = guna2TextBox1.Text;
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(str))
            {
                var tmp = list.FindAll(each => ContainsWord(each.WarehousesName, str) == true).OrderByDescending(each => each.WarehousesId).ToList();
                DataGirdView(tmp);
                guna2Button1.Visible = true;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DataGirdView(list);
            guna2Button1.Visible=false;
            guna2TextBox1.Clear();
        }
    }
}
