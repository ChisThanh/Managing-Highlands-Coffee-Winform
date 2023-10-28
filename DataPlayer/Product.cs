using System;

using System.Data.SqlClient;
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

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string insertQuery = "INSERT INTO Ingredient (product_name, description) VALUES (@a, @b); SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@a", productName);
                        command.Parameters.AddWithValue("@b", description);

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

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string sqlQuery = "SELECT COUNT(*) FROM Ingredient WHERE product_name = @a";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@a", productName);
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sqlQuery = "SELECT product_id FROM Ingredient WHERE product_name = @a";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@a", productName);

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
