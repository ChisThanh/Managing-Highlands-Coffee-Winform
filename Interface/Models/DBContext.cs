using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Interface.Models
{
    public class DBContext
    {
        protected string connectionString = @"Driver={ODBC Driver 18 for SQL Server};
                                        Server=tcp:highlanddb.database.windows.net,1433;
                                        Database=highland;Uid=vudance;Pwd=Vudang0402;
                                        Encrypt=yes;TrustServerCertificate=no;
                                        Connection Timeout=30;";
        public DBContext() { }
        public async Task<bool> ExecuteQuery(string query)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                using (OdbcCommand command = new OdbcCommand(query, connection))
                {
                    try
                    {
                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }
            }
        }
    }
}

    
