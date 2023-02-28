using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OnlineStore.Models
{
    public class SessionInformation
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        public string Id { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int UserId { get; set; }
        public List<OrderDetail> ShoppingCart { get; set; }
    }
}
