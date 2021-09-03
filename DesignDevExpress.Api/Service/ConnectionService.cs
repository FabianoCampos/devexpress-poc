using System.Data.SqlClient;

namespace DesignDevExpress.Api.Service
{
    public class ConnectionService
    {
        public static string GetConnectionString()
        {
            //change to the correct connection string
            var connectionStringBuilder = new SqlConnectionStringBuilder()
            {
                
                DataSource = "localhost",
                InitialCatalog = "dbname",
                UserID = "user",
                Password = "pass"
            };
            
            return $"{connectionStringBuilder}";
        } 
    }
}