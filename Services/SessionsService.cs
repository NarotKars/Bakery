using MongoDB.Bson;
using MongoDB.Driver;
using OnlineStore.Exceptions;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public class SessionsService
    {
        private readonly IConfiguration configuration;
        private readonly ProductsService productsService;
        public string SessionId { get; set; }

        public SessionsService(IConfiguration configuration, ProductsService productsService)
        {
            this.configuration = configuration;
            this.productsService = productsService;
        }

        public async Task RegisterSession(int userId)
        {
            var mongoDb = ConnectionManager.GetMongoDb(configuration);
            var collection = mongoDb.GetCollection<SessionInformation>("SessionInformation");
            var sessionInformation = new SessionInformation()
            {
                UserId = userId,
                StartTime = DateTime.Now
            };
            
            await collection.InsertOneAsync(sessionInformation);
        }

        public async Task StartSession(int userId)
        {
            var mongoDb = ConnectionManager.GetMongoDb(configuration);
            var collection = mongoDb.GetCollection<SessionInformation>("SessionInformation");
            var filter = Builders<SessionInformation>.Filter.Eq(nameof(SessionInformation.UserId), userId);
            var update = Builders<SessionInformation>.Update.Set(nameof(SessionInformation.StartTime), DateTime.Now);
            this.SessionId = (await collection.FindOneAndUpdateAsync(filter, update)).Id;
        }

        public async Task EndSession(int userId)
        {
            var mongoDb = ConnectionManager.GetMongoDb(configuration);
            var collection = mongoDb.GetCollection<SessionInformation>("SessionInformation");
            var filter = Builders<SessionInformation>.Filter.Eq(nameof(SessionInformation.UserId), userId);
            var update = Builders<SessionInformation>.Update.Set(nameof(SessionInformation.EndTime), DateTime.Now);
            await collection.FindOneAndUpdateAsync(filter, update);
            this.SessionId = "";
        }

        public async Task AddProductToShoppingCart(OrderDetail orderDetail, string sessionId)
        {
            var product = (await this.productsService.GetProductsByIdsFromDb(new List<string>() { orderDetail.ProductId })).FirstOrDefault();
            if(product == default)
            {
                throw new RESTException($"A product with the specified {orderDetail.ProductId} id is not found", System.Net.HttpStatusCode.NotFound);
            }

            this.productsService.CheckProductsAvailability(new List<OrderDetail>() { orderDetail }, new List<Product>() { product });

            var mongoDb = ConnectionManager.GetMongoDb(configuration);
            var collection = mongoDb.GetCollection<SessionInformation>("SessionInformation");
            var filter = Builders<SessionInformation>.Filter.Eq(nameof(SessionInformation.Id), sessionId);
            var shoppingCart = (await collection.FindAsync<SessionInformation>(filter)).First().ShoppingCart;
            
            if (shoppingCart.Contains(orderDetail))
            {
                var update = Builders<SessionInformation>.Update.Inc(sessionInfo => sessionInfo.ShoppingCart[-1].Quantity, orderDetail.Quantity);
                await collection.UpdateOneAsync(filter, update);
            }
            else
            {
                var update = Builders<SessionInformation>.Update.Push(nameof(SessionInformation.ShoppingCart), orderDetail);
                await collection.UpdateOneAsync(filter, update);
            }
        }

        public async Task RemoveProductFromShoppingCart(OrderDetail orderDetail, string sessionId)
        {
            var product = (await this.productsService.GetProductsByIdsFromDb(new List<string>() { orderDetail.ProductId })).FirstOrDefault();
            if (product == default)
            {
                throw new RESTException($"A product with the specified {orderDetail.ProductId} id is not found", System.Net.HttpStatusCode.NotFound);
            }

            var mongoDb = ConnectionManager.GetMongoDb(configuration);
            var collection = mongoDb.GetCollection<SessionInformation>("SessionInformation");
            var filter = Builders<SessionInformation>.Filter.Eq(nameof(SessionInformation.Id), sessionId);
            var update = Builders<SessionInformation>.Update.Pull(nameof(SessionInformation.ShoppingCart), orderDetail);
            await collection.FindOneAndUpdateAsync(filter, update);
        }

    }
}
