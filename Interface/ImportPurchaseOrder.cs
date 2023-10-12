using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DataPlayer;
using Interface.Helpers;

namespace Interface
{
    public partial class ImportPurchaseOrder : Form
    {
        private List<Product> products = new List<Product>();
        Supplier supplier = new Supplier();
        List<Supplier> suppliers;
        string total = "";
        public ImportPurchaseOrder()
        {
            InitializeComponent();
        }
        private async void ImportProduct_Load(object sender, EventArgs e)
        {
            //Excel excel = new Excel();
            //products = excel.FileEXProduct("D:\\Code\\Interface\\Excel\\Book1.xlsx");
            dataGirdView();

            suppliers = await supplier.getAllTable();
            guna2ComboBox1.DataSource = suppliers;
            guna2ComboBox1.DisplayMember = "Name";
            guna2ComboBox1.ValueMember = "Id";
            if (guna2DataGridView1.RowCount == 0)
            {
                guna2Button1.Enabled = false;
            }
        }
        private void addRow(Product p)
        {
            int rowIndex = guna2DataGridView1.Rows.Add();
            p.ProductId = rowIndex;
            guna2DataGridView1.Rows[rowIndex].Cells["Column1"].Value = p.ProductId;
            guna2DataGridView1.Rows[rowIndex].Cells["Column2"].Value = p.ProductName;
            guna2DataGridView1.Rows[rowIndex].Cells["Column3"].Value = p.Price;
            guna2DataGridView1.Rows[rowIndex].Cells["Column4"].Value = p.Quantity;
            guna2DataGridView1.Rows[rowIndex].Cells["Column5"].Value = Image.FromFile(@"..\..\images\icons8-edit-50.png");
            guna2DataGridView1.Rows[rowIndex].Cells["Column6"].Value = Image.FromFile(@"..\..\images\icons8-delete-60.png");
        }
        private void dataGirdView()
        {
            ClearDAGV();
            foreach (var product in products)
            {
                addRow(product);
            }

        }
        private void ClearDAGV()
        {
            guna2DataGridView1.DataSource = null;
            guna2DataGridView1.Rows.Clear();
        }
        private void guna2ComboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (guna2DataGridView1.RowCount <= 0 || guna2ComboBox1.SelectedItem == null)
            {
                return;
            }
            guna2Button1.Enabled = true;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var f = new CreateCart("Thêm");
            if (f.ShowDialog() == DialogResult.OK)
            {
                Product newProduct = f.NewProduct;
                newProduct.ProductId = guna2DataGridView1.RowCount;
                products.Add(newProduct);
                addRow(newProduct);
            }
        }
        private void guna2Button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Chọn tệp tin";
            openFileDialog.Filter = "Tất cả các tệp tin|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                Excel excel = new Excel();
                products.AddRange(excel.FileEXProduct(selectedFilePath));

                dataGirdView();
            }
        }
        private void guna2DataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];

                string columnName = guna2DataGridView1.Columns[e.ColumnIndex].Name;

                if (columnName == "Column5")
                {

                    int productId = Convert.ToInt32(selectedRow.Cells["Column1"].Value);
                    string productName = selectedRow.Cells["Column2"].Value.ToString();
                    double price = Convert.ToDouble(selectedRow.Cells["Column3"].Value);
                    int stockQuantity = Convert.ToInt32(selectedRow.Cells["Column4"].Value);


                    var f = new CreateCart("Sửa",
                        (new Product
                        {
                            ProductId = productId,
                            ProductName = productName,
                            Price = price,
                            Quantity = stockQuantity,
                        }));

                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        Product newProduct = f.NewProduct;

                        Product productToEdit = products.FirstOrDefault(p => p.ProductId == newProduct.ProductId);

                        if (productToEdit != null)
                        {
                            productToEdit.ProductName = newProduct.ProductName;
                            productToEdit.Price = newProduct.Price;
                            productToEdit.Quantity = newProduct.Quantity;

                            MessageBox.Show("Sản phẩm đã được sửa thành công!");
                            dataGirdView();
                        }
                        else
                        {
                            MessageBox.Show("Lỗi");
                        }

                    }


                }
               
            }
        }
        private async void guna2Button3_Click(object sender, EventArgs e)
        {
            var f = new CreateSuppliers();
            if (f.ShowDialog() == DialogResult.OK)
            {
                suppliers = await supplier.getAllTable();
                guna2ComboBox1.DataSource = suppliers;
            }
        }
        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thêm thành công", "Thông báo");
            ClearDAGV();
            PurchaseOrder pur = new PurchaseOrder();
            Product product = new Product();
            PurchaseOrderDetail pd = new PurchaseOrderDetail();
            Warehouse warehouse = new Warehouse();


            int SupID = (int)guna2ComboBox1.SelectedValue;

            int OrderID = await pur.InsertPurchaseOrder(SupID, DateTime.Now.ToShortDateString(), this.total);

            if (OrderID != -1)
            {
                foreach (Product item in products)
                {
                    bool isProductExists = await product.IsProductExists(item.ProductName);
                    if (isProductExists)
                    {
                        int ProductID = await product.GetProductIdByName(item.ProductName);
                        bool isInserted = await pd.InsertPurchaseOrderDetail(OrderID.ToString(), ProductID.ToString(), item.Quantity.ToString(), item.Price.ToString());

                        bool checkProduct = await warehouse.isProductInWarehouse(ProductID);
                        if (checkProduct)
                            await warehouse.UpdateProductInWarehouse(ProductID, item.Quantity);
                        else
                            await warehouse.InsertProductInWarehouse(ProductID, item.Quantity);

                        if (!isInserted)
                        {
                            MessageBox.Show("Có lỗi khi thêm chi tiết đơn hàng.");
                            return;
                        }
                    }
                    else
                    {
                        int ProductID = await product.InsertProductAndGetId(item.ProductName, "");
                        if (ProductID != -1)
                        {
                            bool isInserted = await pd.InsertPurchaseOrderDetail(OrderID.ToString(), ProductID.ToString(), item.Quantity.ToString(), item.Price.ToString());

                            bool checkProduct = await warehouse.isProductInWarehouse(ProductID);
                            if (checkProduct)
                                await warehouse.UpdateProductInWarehouse(ProductID, item.Quantity);
                            else
                                await warehouse.InsertProductInWarehouse(ProductID, item.Quantity);
                            if (!isInserted)
                            {
                                MessageBox.Show("Có lỗi khi thêm chi tiết đơn hàng.");
                                return;
                            }
                        }
                        else
                        {
                            await pur.DeleteLastPurchaseOrder();
                            MessageBox.Show("Có lỗi khi thêm sản phẩm mới.");
                            return;
                        }
                    }
                }
            }
            
        }
        private void guna2DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (!guna2Button1.Enabled)
                guna2Button1.Enabled = true;
            this.total = products.Sum(x => x.Price).ToString();

            label2.Text = FormatCurrency.FormatAmount(int.Parse(this.total));
        }
        private void guna2DataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (guna2DataGridView1.RowCount == 0)
            {
                guna2Button1.Enabled = false;
            }
            this.total = products.Sum(x => x.Price).ToString();
            label2.Text = FormatCurrency.FormatAmount(int.Parse(this.total));
        }
        private bool isDataGridViewValueChanged = false;
        private void guna2DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            isDataGridViewValueChanged = true;
            if (isDataGridViewValueChanged)
            {
                this.total = products.Sum(x => x.Price).ToString();
                label2.Text = FormatCurrency.FormatAmount(int.Parse(this.total));
                isDataGridViewValueChanged = false;
            }
           
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = guna2DataGridView1.Columns[e.ColumnIndex].Name;
            if (columnName == "Column6")
            {
                int selectedRowIndex = e.RowIndex;
                DataGridViewCell selectedCell = guna2DataGridView1.Rows[selectedRowIndex].Cells[0];
                string id = selectedCell.Value.ToString();


                Product tmp = products.FirstOrDefault(i => i.ProductId == int.Parse(id));
                products.Remove(tmp);

                DialogResult d = MessageBox.Show("Bạn có chắc chắn muốn xóa", "Cảnh báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (d == DialogResult.OK)
                {
                    DataGridViewRow selectedRow = guna2DataGridView1.SelectedRows[0];
                    guna2DataGridView1.Rows.Remove(selectedRow);
                }
            }

        }
    }
}

