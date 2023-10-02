using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interface.Models
{
    internal class PurchaseOrder : DBContext
    {
        public int order_id { get; set; }
        public int supplier_id { get; set; }
        public DateTime order_date { get; set; }

        public async Task<int> InsertPurchaseOrder(int supplierId, string orderDate, string total)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string insertQuery = "INSERT INTO PurchaseOrders (supplier_id, order_date, total) VALUES (?, ?, ?); SELECT SCOPE_IDENTITY();";


                    using (OdbcCommand command = new OdbcCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("?", supplierId);
                        command.Parameters.AddWithValue("?", orderDate);
                        command.Parameters.AddWithValue("?", int.Parse(total));

                        int newId = Convert.ToInt32(await command.ExecuteScalarAsync());

                        return newId;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            return -1;
        }

        public async Task<List<PurchaseOrder>> GetAllPurchaseOrders()
        {
            List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string sqlQuery = "SELECT * FROM PurchaseOrders";

                    using (OdbcCommand command = new OdbcCommand(sqlQuery, connection))
                    {
                        using (OdbcDataReader reader = (OdbcDataReader)await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                PurchaseOrder purchaseOrder = new PurchaseOrder();
                                purchaseOrder.order_id = reader.GetInt32(0);
                                purchaseOrder.supplier_id = reader.GetInt32(1);
                                purchaseOrder.order_date = reader.GetDateTime(2);

                                purchaseOrders.Add(purchaseOrder);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            return purchaseOrders;
        }


        public async Task<List<Tuple<int, string, DateTime, int>>> GetPurchaseOrdersWithSupplierName()
        {
            List<Tuple<int, string, DateTime, int>> purchaseOrdersWithSupplierInfo = new List<Tuple<int, string, DateTime,int>>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string sqlQuery = "SELECT p.order_id, s.supplier_name, p.order_date, p.total FROM suppliers s " +
                                      "JOIN purchaseorders p ON s.supplier_id = p.supplier_id";

                    using (OdbcCommand command = new OdbcCommand(sqlQuery, connection))
                    {
                        using (OdbcDataReader reader = (OdbcDataReader)await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                int orderId = reader.GetInt32(0);
                                string supplierName = reader.GetString(1);
                                DateTime orderDate = reader.GetDateTime(2);
                                int total = reader.GetInt32(3);

                                Tuple<int, string, DateTime,int> orderInfo = new Tuple<int, string, DateTime,int>(orderId, supplierName, orderDate, total);
                                purchaseOrdersWithSupplierInfo.Add(orderInfo);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            return purchaseOrdersWithSupplierInfo;
        }

    }
}
