using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Threading.Tasks;


namespace DataPlayer
{
    public class Supplier : DBContext
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public async Task<List<Supplier>> getAllTable()
        {
            List<Supplier> tmp = new List<Supplier>();
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string sqlQuery = "SELECT * FROM Suppliers";
                    using (OdbcCommand command = new OdbcCommand(sqlQuery, connection))
                    {
                        using (OdbcDataReader reader = (OdbcDataReader)await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Supplier supplier = new Supplier();
                                supplier.Id = reader.GetInt32(0);
                                supplier.Name = reader.GetString(1);
                                supplier.Email = reader.GetString(1);
                                supplier.Phone = reader.GetString(1);
                                tmp.Add(supplier);
                            }
                        }
                    }
                    return tmp;
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
        public bool InsertSupplier(string name, string email, string phone)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sqlQuery = "INSERT INTO Suppliers (supplier_name, contact_email, contact_phone) VALUES (?, ?, ?)";

                    using (OdbcCommand command = new OdbcCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("?", name);
                        command.Parameters.AddWithValue("?", email);
                        command.Parameters.AddWithValue("?", phone);

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
