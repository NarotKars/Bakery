using MongoDB.Bson;

namespace OnlineStore.Models
{
    public class OrderDetail
    {
        public string ProductId { get; set; }
        public decimal Quantity { get; set; }
    }
}
