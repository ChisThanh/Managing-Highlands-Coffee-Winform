using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string checkExistingQuery = "SELECT COUNT(*) FROM PurchaseOrderDetails WHERE order_id = @a AND product_id = @b";

                    using (SqlCommand command = new SqlCommand(checkExistingQuery, connection))
                    {
                        command.Parameters.AddWithValue("@a", OrderId);
                        command.Parameters.AddWithValue("@b", ProductId);

                        int existingCount = (int)await command.ExecuteScalarAsync();

                        if (existingCount == 0)
                        {
                            string insertQuery = "INSERT INTO PurchaseOrderDetails (order_id, product_id, quantity, price) VALUES (@a, @b, @c, @d)";
                            using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@a", OrderId);
                                insertCommand.Parameters.AddWithValue("@b", ProductId);
                                insertCommand.Parameters.AddWithValue("@c", Quantity);
                                insertCommand.Parameters.AddWithValue("@d", Price);

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

            using (SqlConnection connection = new SqlConnection(connectionString))
            {     
                try
                {
                    await connection.OpenAsync();

                    string sqlQuery = "SELECT d.product_name, p.quantity, p.price " +
                                      "FROM PurchaseOrderDetails p " +
                                      "JOIN Ingredient d ON p.product_id = d.product_id " +
                                      "WHERE P.order_id = @OrderID";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@OrderID", order_id);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
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
