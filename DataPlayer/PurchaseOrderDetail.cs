using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Threading.Tasks;


namespace DataPlayer
{
    public class PurchaseOrderDetail : DBContext
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }


        public async Task<bool> InsertPurchaseOrderDetail(string OrderId, string ProductId, string Quantity, string Price)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string checkExistingQuery = "SELECT COUNT(*) FROM PurchaseOrderDetails WHERE order_id = ? AND product_id = ?";

                    using (OdbcCommand checkCommand = new OdbcCommand(checkExistingQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("?", OrderId);
                        checkCommand.Parameters.AddWithValue("?", ProductId);

                        int existingCount = (int)await checkCommand.ExecuteScalarAsync();

                        if (existingCount == 0)
                        {
                            string insertQuery = "INSERT INTO PurchaseOrderDetails (order_id, product_id, quantity, price) VALUES (?, ?, ?, ?)";
                            using (OdbcCommand insertCommand = new OdbcCommand(insertQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("?", OrderId);
                                insertCommand.Parameters.AddWithValue("?", ProductId);
                                insertCommand.Parameters.AddWithValue("?", Quantity);
                                insertCommand.Parameters.AddWithValue("?", Price);

                                int rowsAffected = await insertCommand.ExecuteNonQueryAsync();

                                return rowsAffected > 0;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<List<Tuple<string, int, double>>> GetProductDetails(string order_id)
        {
            List<Tuple<string, int, double>> productDetails = new List<Tuple<string, int, double>>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string sqlQuery = "SELECT d.product_name, p.quantity, p.price " +
                                      "FROM PurchaseOrderDetails p " +
                                      "JOIN Products d ON p.product_id = d.product_id " +
                                      "WHERE P.order_id = ?";

                    using (OdbcCommand command = new OdbcCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("?", order_id);

                        using (OdbcDataReader reader = (OdbcDataReader)await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                string productName = reader.GetString(0);
                                int quantity = reader.GetInt32(1);
                                double price = reader.GetDouble(2);

                                Tuple<string, int, double> productDetail = new Tuple<string, int, double>(productName, quantity, price);
                                productDetails.Add(productDetail);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return productDetails;
        }

    }

}
