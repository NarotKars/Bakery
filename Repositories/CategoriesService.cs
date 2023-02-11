using System.Data;
using OnlineStore.Models;
using Dapper;

namespace OnlineStore.Repositories
{
    public class CategoriesService
    {
        private readonly IConfiguration configuration;

        public CategoriesService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            string sql = "GetCategories";
            return await ConnectionManager.CreateConnection(configuration)
                                          .ExecuteScalarAsync<List<Category>>(sql, commandType: CommandType.StoredProcedure);
        }
    }
}
