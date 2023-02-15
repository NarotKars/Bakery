using OnlineStore.Enums;
namespace OnlineStore.Models
{
    public class Order
    {
        public List<OrderDetail> Details { get; set; }
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
