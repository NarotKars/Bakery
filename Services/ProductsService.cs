using System.Data;
using OnlineStore.Models;
using Dapper;
using OnlineStore.Exceptions;
using System.Data.Common;
using System.Data.SqlClient;

namespace OnlineStore.Services
{
    public class ProductsService
    {
        private readonly IConfiguration configuration;

        public ProductsService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            string sql = "GetProducts";
            return await ConnectionManager.CreateConnection(configuration)
                                          .QueryAsync<Product>(sql, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId)
        {
            string sql = "GetProducts";
            return await ConnectionManager.CreateConnection(configuration)
                                          .QueryAsync<Product>(sql, new { CategoryId = categoryId }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Product>> GetProductsByIds(string ids, SqlConnection connection, DbTransaction transaction = null)
        {
            string sql = "GetProducts";
            if(connection == null)
            {
                connection = ConnectionManager.CreateConnection(configuration);
            }
            var product = await connection.QueryAsync<Product>(sql, new { ProductIds = ids }, transaction, commandType: CommandType.StoredProcedure);
            if(product == default)
            {
                throw new RESTException("The sepcified products are not found", System.Net.HttpStatusCode.NotFound);
            }

            return product;

        }
    }
}