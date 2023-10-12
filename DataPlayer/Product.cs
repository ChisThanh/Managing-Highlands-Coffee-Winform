using System;
using System.Data.Odbc;
using System.Threading.Tasks;

namespace DataPlayer
{
    public class Product : DBContext
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }


        public async Task<int> InsertProductAndGetId(string productName, string description)
        {
            int newProductId = -1;

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string insertQuery = "INSERT INTO Products (product_name, description) VALUES (?, ?); SELECT SCOPE_IDENTITY();";

                    using (OdbcCommand command = new OdbcCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("?", productName);
                        command.Parameters.AddWithValue("?", description);

                        newProductId = Convert.ToInt32(await command.ExecuteScalarAsync());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return newProductId;
        }
        public async Task<bool> IsProductExists(string productName)
        {

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string sqlQuery = "SELECT COUNT(*) FROM Products WHERE product_name = ?";
                    using (OdbcCommand command = new OdbcCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("?", productName);
                        int productCount = (int)command.ExecuteScalar();
                        if (productCount > 0)
                        {
                            return true;
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

                return false;
            }
        }
        public async Task<int> GetProductIdByName(string productName)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sqlQuery = "SELECT product_id FROM Products WHERE product_name = ?";

                    using (OdbcCommand command = new OdbcCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("?", productName);

                        object result = await command.ExecuteScalarAsync();

                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return -1;
        }

    }
}
