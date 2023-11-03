using Guna.UI2.WinForms;
using Interface.Helpers;
using Interface.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using static OfficeOpenXml.ExcelErrorValue;

namespace Interface
{
    public partial class ProductTransfers : Form
    {
        public ProductTransfers()
        {
            InitializeComponent();
        }
        int x = 125;
        int y = 17;
        int controlCounter = 0;
        int warehouse_id = -1;
        List<GetAllWareHouseById_Result> list = new List<GetAllWareHouseById_Result>();

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if(panel1.Controls.Count != controlCounter)
            {
                int newY = 0;
                int ctmp = 0;
                foreach (Control control in panel1.Controls.OfType<ControlProduct>())
                {
                    control.Location = new Point(control.Location.X, newY);
                    newY += control.Height;
                    ctmp++;
                }
                controlCounter = ctmp;
            }

            GetListProduct();
            warehouse_id = (int)guna2ComboBox2.SelectedValue;

            ControlProduct a = new ControlProduct(warehouse_id, list);

            a.Location = new Point(x, y + (controlCounter * 70));
            a.Size = new Size(700, 75);
            panel1.Controls.Add(a);

            controlCounter++;
        }
       

        HighlandEntities db = new HighlandEntities();
        private void ProductTransfers_Load(object sender, EventArgs e)
        {
            var branchs = db.GetWarehouse().ToList();

            branchs.Reverse();
            foreach (var i in branchs)
            {
                i.name = HP.GetLast2Words(i.name);
            }
            guna2ComboBox2.DataSource = branchs;
            guna2ComboBox2.DisplayMember = "name";
            guna2ComboBox2.ValueMember = "id";
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            controlCounter = 0;
            var branchs = db.Branches.ToList();

            branchs.Reverse();


            foreach (var i in branchs)
            {
                if (warehouse_id == i.Id)
                    continue;
                i.Name = HP.GetLast2Words(i.Name);
            }
            guna2ComboBox1.DataSource = branchs;
            guna2ComboBox1.DisplayMember = "name";
            guna2ComboBox1.ValueMember = "id";
        }

        private void GetListProduct()
        {
            List<ControlProduct> controlProductList = new List<ControlProduct>();

            foreach (Control control in panel1.Controls)
            {
                if (control is ControlProduct)
                {
                    controlProductList.Add((ControlProduct)control);
                }
            }

            foreach (ControlProduct i in controlProductList)
            {
                list.Add(i.ResultP());
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            list = new List<GetAllWareHouseById_Result>();
            GetListProduct();
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("Chưa có sản phẩm nào");
                return;
            }
            try
            {
                int from = (int)guna2ComboBox2.SelectedValue; ;
                int to = (int)guna2ComboBox1.SelectedValue; ;
                ProductTransfer newTransfer = new ProductTransfer
                {
                    from_warehouse_id = from,
                    to_warehouse_id = to,
                    transfer_date = DateTime.Now
                };

                db.ProductTransfers.Add(newTransfer);
                db.SaveChanges();
                int TransferID = newTransfer.transfer_id;

                foreach (var item in list)
                {
                    db.issue(from,to, TransferID, item.product_id, item.quantity);
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Xuất kho thành công!");
            panel1.Controls.Clear();
        }
    }
}
