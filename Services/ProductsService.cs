using System.Data;
using OnlineStore.Models;
using Dapper;

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
    }
}