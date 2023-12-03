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

                    string sqlQuery = "SELECT * FROM Branch";

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

        public async Task<List<Tuple<string, int>>> GetProductInWarehouse(string id)
        {
            List<Tuple<string, int>> productQuantities = new List<Tuple<string, int>>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string sqlQuery = $"SELECT product_name , w.quantity from WarehousesDetails w join Ingredient i on w.product_id = i.product_id where w.warehouses_id = {id} and  w.quantity > 0 ";
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

    }
}
