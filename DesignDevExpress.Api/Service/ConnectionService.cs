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
                
                DataSource = "172.21.24.4,41433",
                InitialCatalog = "UNJ01TINT",
                UserID = "sajtools",
                Password = "@agesune1"
            };
            
            return $"{connectionStringBuilder}";
        } 
    }
}