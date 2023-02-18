using Azure.Storage.Blobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
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
            builder.DataSource = configuration["DbConnectionString:DataSource"];
            builder.InitialCatalog = configuration["DbConnectionString:InitialCatalog"];
            builder.UserID = configuration["DbConnectionString:UserName"];
            builder.Password = Encoding.UTF8.GetString(Convert.FromBase64String(configuration["DbConnectionString:PasswordHash"]));
            ConnectionString = builder.ConnectionString;
            return new SqlConnection(ConnectionString);
        }

        public static BlobContainerClient GetBlobContainerClient(IConfiguration configuration, string container)
        {
            var storageCredentials = new StorageCredentials(configuration["BlobConnectionString:AccountName"]
                                                          , Encoding.UTF8.GetString(Convert.FromBase64String(configuration["BlobConnectionString:AccountKeyHash"])));

            var storageAccount = new CloudStorageAccount(storageCredentials, useHttps:true);
            var blobServiceClient = new BlobServiceClient(storageAccount.ToString(true));
            if(!blobServiceClient.GetBlobContainerClient(container).Exists())
            {
                blobServiceClient.CreateBlobContainer(container, Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            }
            return new BlobContainerClient(storageAccount.ToString(true), container);
        }
    }
}