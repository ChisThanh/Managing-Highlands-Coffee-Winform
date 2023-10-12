using System;
using System.Collections.Generic;
using System.Data.Odbc;
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

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string sqlQuery = "SELECT * FROM Warehouses";

                    using (OdbcCommand command = new OdbcCommand(sqlQuery, connection))
                    {
                        using (OdbcDataReader reader = (OdbcDataReader)await command.ExecuteReaderAsync())
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

            using (OdbcConnection connection = new OdbcConnection(connectionString))
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

                    using (OdbcCommand command = new OdbcCommand(sqlQuery, connection))
                    {
                        using (OdbcDataReader reader = (OdbcDataReader)await command.ExecuteReaderAsync())
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
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string insertQuery = "INSERT INTO ProductTransferDetails (transfer_id, product_id, quantity) VALUES (1, ?, ?)";

                    using (OdbcCommand command = new OdbcCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("?", productId);
                        command.Parameters.AddWithValue("?", quantity);

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
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string selectQuery = "SELECT * FROM ProductTransferDetails WHERE transfer_id = 1 AND product_id = ?";
                    using (OdbcCommand selectCommand = new OdbcCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("?", productId);

                        using (OdbcDataReader reader = (OdbcDataReader)await selectCommand.ExecuteReaderAsync())
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
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string updateQuery = "UPDATE ProductTransferDetails SET quantity = quantity + ? WHERE transfer_id = 1 AND product_id = ?";
                    using (OdbcCommand updateCommand = new OdbcCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("?", quantity);
                        updateCommand.Parameters.AddWithValue("?", productId);

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
