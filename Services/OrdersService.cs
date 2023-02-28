using OnlineStore.Models;
using System.Data;
using MongoDB.Driver;

namespace OnlineStore.Services
{
    public class OrdersService
    {
        private readonly IConfiguration configuration;
        private readonly ProductsService productsService;

        public OrdersService(IConfiguration configuration, ProductsService productsService)
        {
            this.configuration = configuration;
            this.productsService = productsService;
        }

        public async Task<string> CreateOrder(OrderStoreParams orderParams)
        {
            var products = await this.productsService.GetProductsByIds(orderParams.Details.Select(detail => detail.ProductId));
            this.productsService.CheckProductsAvailability(orderParams.Details, products);

            var client = ConnectionManager.GetMongoClient(configuration);
            var mongoDb = client.GetDatabase("bakery");
            using var session = client.StartSession();

            var collection = mongoDb.GetCollection<Order>("Orders");

            var order = new Order()
            {
                Details = orderParams.Details,
                AddressId = orderParams.AddressId,
                OrderDate = orderParams.Date,
                Status = orderParams.Status,
                UserId = orderParams.UserId
            };
            await collection.InsertOneAsync(order);

            var productsColleection = mongoDb.GetCollection<Product>("Products");
            foreach (var orderDetails in orderParams.Details)
            {
                var update = Builders<Product>.Update.Inc(nameof(Product.Quantity), -orderDetails.Quantity);
                var filter = Builders<Product>.Filter.Eq(nameof(Product.Id), orderDetails.ProductId);
                await productsColleection.UpdateOneAsync(filter, update);
            }

            session.CommitTransaction();
            return order.Id;
        }

        public async Task<Order> GetOrder(string id)
        {
            var mongoDb = ConnectionManager.GetMongoDb(configuration);
            var collection = mongoDb.GetCollection<Order>("Orders");
            var filter = Builders<Order>.Filter.Eq(nameof(Order.Id), id);
            return (await collection.FindAsync(filter)).FirstOrDefault();
        }

        public async Task<IEnumerable<Order>> GetOrderByUserId(string userId)
        {
            var mongoDb = ConnectionManager.GetMongoDb(configuration);
            var collection = mongoDb.GetCollection<Order>("Orders");
            var filter = Builders<Order>.Filter.Eq(nameof(Order.UserId), userId);
            return (await collection.FindAsync(filter)).ToEnumerable();
        }
    }
}
