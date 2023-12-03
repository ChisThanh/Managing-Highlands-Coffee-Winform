using System;
using System.Collections.Generic;

using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPlayer
{
    public class PurchaseOrder : DBContext
    {
        public int order_id { get; set; }
        public int supplier_id { get; set; }
        public DateTime order_date { get; set; }

        public async Task<int> InsertPurchaseOrder(int supplierId, string orderDate, int total)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string insertQuery = "SET DATEFORMAT dmy INSERT INTO PurchaseOrders (supplier_id, order_date, total) VALUES (@a, @b, @c); SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@a", supplierId);
                        command.Parameters.AddWithValue("@b", orderDate);
                        command.Parameters.AddWithValue("@c", total);

                        int newId = Convert.ToInt32(await command.ExecuteScalarAsync());

                        return newId;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }


        public async Task<List<PurchaseOrder>> GetAllPurchaseOrders()
        {
            List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string sqlQuery = "SELECT * FROM PurchaseOrders";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
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
                    throw new Exception(ex.Message);
                }
            }

            return purchaseOrders;
        }


        public async Task<List<Tuple<int, string, DateTime, int>>> GetPurchaseOrdersWithSupplierName()
        {
            List<Tuple<int, string, DateTime, int>> purchaseOrdersWithSupplierInfo = new List<Tuple<int, string, DateTime,int>>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {     
                try
                {
                    await connection.OpenAsync();

                    string sqlQuery = "SELECT p.order_id, s.supplier_name, p.order_date, p.total FROM suppliers s " +
                                      "JOIN purchaseorders p ON s.supplier_id = p.supplier_id";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
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
                    throw new Exception(ex.Message);
                }
            }

            return purchaseOrdersWithSupplierInfo;
        }

        public async Task<bool> DeleteLastPurchaseOrder()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string findLastOrderIdQuery = "SELECT TOP 1 order_id FROM PurchaseOrders ORDER BY order_id DESC";
                    using (SqlCommand findLastOrderIdCommand = new SqlCommand(findLastOrderIdQuery, connection))
                    {
                        int lastOrderId = (int)await findLastOrderIdCommand.ExecuteScalarAsync();

                        string deleteLastOrderQuery = "DELETE FROM PurchaseOrders WHERE order_id = @a";
                        using (SqlCommand deleteLastOrderCommand = new SqlCommand(deleteLastOrderQuery, connection))
                        {
                            deleteLastOrderCommand.Parameters.AddWithValue("@a", lastOrderId);
                            int rowsAffected = await deleteLastOrderCommand.ExecuteNonQueryAsync();

                            return rowsAffected > 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);

                }
            }
        }


    }
}
