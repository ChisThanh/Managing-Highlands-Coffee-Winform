using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;



namespace DataPlayer
{
    public class Supplier : DBContext
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public async Task<List<Supplier>> GetAllTable()
        {
            List<Supplier> suppliers = new List<Supplier>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string sqlQuery = "SELECT * FROM Suppliers";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Supplier supplier = new Supplier();
                                supplier.Id = reader.GetInt32(0);
                                supplier.Name = reader.GetString(1);
                                supplier.Email = reader.GetString(2); // Sử dụng cột thứ 2 cho Email
                                supplier.Phone = reader.GetString(3); // Sử dụng cột thứ 3 cho Số điện thoại
                                suppliers.Add(supplier);
                            }
                        }
                    }

                    return suppliers;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public bool InsertSupplier(string name, string email, string phone)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sqlQuery = "INSERT INTO Suppliers (supplier_name, contact_email, contact_phone) VALUES (@a, @b, @c)";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@a", name);
                        command.Parameters.AddWithValue("@b", email);
                        command.Parameters.AddWithValue("@c", phone);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
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
            }
            return false;
        }

    }


}
