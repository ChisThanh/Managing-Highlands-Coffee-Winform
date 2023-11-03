using System;
using System.Threading.Tasks;

namespace DataPlayer
{
    public class DBContext
    {
       

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

    
