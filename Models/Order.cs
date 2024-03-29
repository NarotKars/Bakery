﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OnlineStore.Enums;
namespace OnlineStore.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public List<OrderDetail> Details { get; set; }
        public string AddressId { get; set; }
        public int UserId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
