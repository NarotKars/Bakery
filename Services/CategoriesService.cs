using System.Data;
using OnlineStore.Models;
using Dapper;

namespace OnlineStore.Services
{
    public class CategoriesService
    {
        private readonly IConfiguration configuration;

        public CategoriesService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            string sql = "GetCategories";
            return await ConnectionManager.CreateConnection(configuration)
                                          .QueryAsync<Category>(sql, commandType: CommandType.StoredProcedure);
        }
    }
}
