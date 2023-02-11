using System.Data.SqlClient;
using System.Text;

namespace OnlineStore
{
    public class ConnectionManager
    {
        private static string ConnectionString { get; set; }

        public static void CheckConnection()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException)
                {
                    throw new Exception("Incorrect username or password. Please try again");
                }
            }
        }

        public static SqlConnection CreateConnection(IConfiguration configuration)
        {
            SqlConnectionStringBuilder builder = new();
            builder.DataSource = configuration["ConnectionString:DataSource"];
            builder.InitialCatalog = configuration["ConnectionString:InitialCatalog"];
            builder.UserID = configuration["ConnectionString:UserName"];
            builder.Password = Encoding.UTF8.GetString(Convert.FromBase64String(configuration["ConnectionString:PasswordHash"]));
            ConnectionString = builder.ConnectionString;
            return new SqlConnection(ConnectionString);
        }
    }
}