using OnlineStore.Models;
using System.Data;
using Dapper;
using OnlineStore.Exceptions;

namespace OnlineStore.Services
{
    public class UsersService
    {
        private readonly IConfiguration configuration;

        public UsersService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<int> Login(User user)
        {
            string sql = "GetUser";
            var id = await ConnectionManager.CreateConnection(configuration)
                                            .QuerySingleOrDefaultAsync<int>(sql, new { UserName = user.UserName, PasswordHash = user.PasswordHash }, commandType: CommandType.StoredProcedure);
            if(id == default)
            {
                throw new RESTException("Invalid user name or password", System.Net.HttpStatusCode.OK);
            }

            return id;
        }

        public async Task<int> Register(User user)
        {
            var sql = "GetUser";
            var id = await ConnectionManager.CreateConnection(configuration)
                                            .QuerySingleOrDefaultAsync<int>(sql, new { UserName = user.UserName }, commandType: CommandType.StoredProcedure);
            if(id != default)
            {
                throw new RESTException(string.Format("A user with name {0} already exists", user.UserName), System.Net.HttpStatusCode.OK);
            }

            sql = "RegisterUser";
            var parameters = new DynamicParameters();
            parameters.Add("UserName", user.UserName);
            parameters.Add("PasswordHash", user.PasswordHash);
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Output);
            await ConnectionManager.CreateConnection(configuration)
                                   .ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("Id");
        }
    }
}
