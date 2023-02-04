using OnlineStore.Enums;
namespace OnlineStore.Models
{
    public class Order
    {
        public List<OrderDetail> OrderDetails { get; set; }
        public int AddressId { get; set; }
    }
}
