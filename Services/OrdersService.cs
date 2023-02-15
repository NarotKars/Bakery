using OnlineStore.Models;
using System.Data;
using Dapper;
using OnlineStore.Enums;
using OnlineStore.Exceptions;
using System.Data.SqlClient;
using System.Data.Common;

namespace OnlineStore.Services
{
    public class OrdersService
    {
        private readonly IConfiguration configuration;
        private readonly ProductsService productsService;
        private SqlConnection connection;
        private DbTransaction transaction;

        public OrdersService(IConfiguration configuration, ProductsService productsService)
        {
            this.configuration = configuration;
            this.productsService = productsService;
        }

        public async Task<int> CreateOrder(Order order)
        {
            await CheckSqlConnectionToOpen();
            await CheckTransactionToBegin();

            string sql = "CreateOrder";
            var parameters = new DynamicParameters();
            parameters.Add("AddressId", order.AddressId);
            parameters.Add("Status", order.Status);
            parameters.Add("UserId", order.UserId);
            parameters.Add("OrderDate", order.OrderDate);
            parameters.Add("Status", order.Status);
            parameters.Add("Amount", await CalculateOrderAmount(order.Details));
            parameters.Add("Id", null, DbType.Int32, ParameterDirection.Output);
            await connection.ExecuteAsync(sql, parameters, transaction, commandType: CommandType.StoredProcedure);
            var orderId = parameters.Get<int>("Id");
            if(orderId <= 0)
            {
                await transaction.RollbackAsync();
                throw new Exception("Something went wrong, please try again");
            }

            sql = "AddProductsToOrder";
            foreach (var detail in order.Details)
            {
                await this.AddOrderDetail(orderId, detail, false);
            }
            await CommitTransaction();
            return orderId;
        }

        public async Task<Order> GetOrder(int id)
        {
            await CheckSqlConnectionToOpen();
            string sql = "GetOrder";
            var order = await connection.QueryFirstOrDefaultAsync<(DateTime OrderDate, decimal Amount, int UserId, int AddressId, OrderStatus Status)>(sql, new { Id = id }, transaction, commandType: CommandType.StoredProcedure);
            if(order == default)
            {
                throw new RESTException("There is no order with the given id", System.Net.HttpStatusCode.NotFound);
            }
            var orderDetails = await this.GetOrderDetails(id);
            return new Order()
            {
                AddressId = order.AddressId,
                Status = order.Status,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Details = orderDetails
            };
        }

        public async Task<List<OrderDetail>> GetOrderDetails(int id)
        {
            await CheckSqlConnectionToOpen();

            var sql = "GetOrderDetails";
            var orderDetails = await connection.QueryAsync<OrderDetail>(sql, new { Id = id }, transaction, commandType: CommandType.StoredProcedure);
            if (!orderDetails.Any())
            {
                throw new Exception("The order has no products in it");
            }

            return orderDetails.ToList();
        }

        public async Task AddOrderDetail(int orderId, OrderDetail orderDetail, bool recalculateOrderAmount = true, bool commitTransaction = true)
        {
            await CheckSqlConnectionToOpen();
            await CheckTransactionToBegin();

            var sql = "AddProductsToOrder";
            var parameters = new DynamicParameters();
            parameters.Add("OrderId", orderId);
            parameters.Add("ProductId", orderDetail.ProductId);
            parameters.Add("Quantity", orderDetail.Quantity);
            parameters.Add("Id", null, DbType.Int32, ParameterDirection.Output);
            await connection.ExecuteAsync(sql, parameters, transaction, commandType: CommandType.StoredProcedure);
            if (parameters.Get<int>("Id") <= 0)
            {
                await RollbackTransaction();
                throw new Exception("Something went wrong, please try again");
            }

            if(recalculateOrderAmount)
            {
                sql = "UpdateOrderAmount";
                await connection.ExecuteAsync(sql, new { OrderId = orderId, Amount = await CalculateOrderAmount((await this.GetOrder(orderId)).Details) }, transaction, commandType: CommandType.StoredProcedure);
            }

            if(commitTransaction)
            {
                await CommitTransaction();
            }
        }

        private async Task<decimal> CalculateOrderAmount(List<OrderDetail> orderDetails)
        {
            var productIds = string.Join(',', orderDetails.Select(o => o.ProductId));
            var products = await this.productsService.GetProductsByIds(productIds, connection, transaction);
            if(products.Count() > orderDetails.Count())
            {
                throw new RESTException("Some of the provided product ids are not found", System.Net.HttpStatusCode.NotFound);
            }
            return products.Sum(product => product.Price * orderDetails.Where(o => o.ProductId == product.Id).Select(o => o.Quantity).First()) ;
        }

        private async Task CheckSqlConnectionToOpen()
        {
            if(connection == null)
            {
                connection = ConnectionManager.CreateConnection(configuration);
                await connection.OpenAsync();
            }
            else if(connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }
        }

        private async Task CheckTransactionToBegin()
        {
            if(transaction == null || transaction.Connection == null || transaction.Connection.State != ConnectionState.Open)
            {
                transaction = await connection.BeginTransactionAsync();
            }
        }

        private async Task CommitTransaction()
        {
            await this.transaction.CommitAsync();
            await this.connection.CloseAsync();
        }

        private async Task RollbackTransaction()
        {
            await this.transaction.RollbackAsync();
            await this.connection.CloseAsync();
        }
    }
}
