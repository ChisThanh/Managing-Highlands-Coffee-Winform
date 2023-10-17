using System;
using System.Threading.Tasks;

namespace DataPlayer
{
    public class DBContext
    {
        //protected string connectionString = @"Driver={ODBC Driver 18 for SQL Server};
        //                                Server=tcp:highlanddb.database.windows.net,1433;
        //                                Database=highland;Uid=vudance;Pwd=Vudang0402;
        //                                Encrypt=yes;TrustServerCertificate=no;
        //                                Connection Timeout=30;";

        protected string connectionString = "Data Source=highlanddb.database.windows.net;" +
                              "Initial Catalog=highland;User ID=vudance;" +
                              "Password=Vudang0402;" +
                              "Connect Timeout=60;" +
                              "Encrypt=True;" +
                              "TrustServerCertificate=False;" +
                              "TrustServerCertificate=False;" +
                              "ApplicationIntent=ReadWrite;" +
                              "MultiSubnetFailover=False";

        public DBContext() { }
      
    }
}

    
