using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataPlayer
{
    public class Warehouse : DBContext
    {
        public int WarehousesId { get; set; }
        public string WarehousesName { get; set; }
        public string Location { get; set; }

        public async Task<List<Warehouse>> GetAll()
        {
            List<Warehouse> warehouses = new List<Warehouse>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string sqlQuery = "SELECT * FROM Warehouses";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                int warehouseId = reader.GetInt32(0);
                                string warehouseName = reader.GetString(1);
                                string location = reader.GetString(2);

                                Warehouse warehouse = new Warehouse
                                {
                                    WarehousesId = warehouseId,
                                    WarehousesName = warehouseName,
                                    Location = location
                                };

                                warehouses.Add(warehouse);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return warehouses;
        }

        public async Task<List<Tuple<string, int>>> GetProductInWarehouse()
        {
            List<Tuple<string, int>> productQuantities = new List<Tuple<string, int>>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string sqlQuery = "SELECT p.product_name, d.quantity " +
                                      "FROM Warehouses w " +
                                      "JOIN ProductTransfers t ON w.warehouse_id = t.to_warehouse_id " +
                                      "JOIN ProductTransferDetails d ON t.transfer_id = d.transfer_id " +
                                      "JOIN Products p ON p.product_id = d.product_id " +
                                      "WHERE w.warehouse_id = 1";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                string productName = reader.GetString(0);
                                int quantity = reader.GetInt32(1);

                                Tuple<string, int> productQuantity = new Tuple<string, int>(productName, quantity);
                                productQuantities.Add(productQuantity);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return productQuantities;
        }

        public async Task<bool> InsertProductInWarehouse(int productId, int quantity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string insertQuery = "INSERT INTO ProductTransferDetails (transfer_id, product_id, quantity) VALUES (1, @a, @b)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@a", productId);
                        command.Parameters.AddWithValue("@b", quantity);

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

        }

        public async Task<bool> isProductInWarehouse(int productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string selectQuery = "SELECT * FROM ProductTransferDetails WHERE transfer_id = 1 AND product_id = @a";
                    using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@a", productId);

                        using (SqlDataReader reader = await selectCommand.ExecuteReaderAsync())
                        {
                            return reader.HasRows;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        public async Task<bool> UpdateProductInWarehouse(int productId, int quantity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string updateQuery = "UPDATE ProductTransferDetails SET quantity = quantity + @a WHERE transfer_id = 1 AND product_id = @b";
                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@a", quantity);
                        updateCommand.Parameters.AddWithValue("@b", productId);

                        int rowsAffected = await updateCommand.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

        }

    }
}
