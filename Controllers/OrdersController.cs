using Microsoft.AspNetCore.Mvc;
using OnlineStore.Enums;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {

        public OrdersController()
        {
        }

        [HttpPost("Create")]
        public ActionResult CreateOrder(Order order)
        {
            return Ok("Your order is successfully created");
        }

        [HttpGet("{id}")]
        public Order GetOrder(int id)
        {
            return new Order()
            {
                OrderDetails = new List<OrderDetail>()
                {
                    new OrderDetail()
                    {
                        ProductId = 1,
                        UserId = 1
                    }
                },
                AddressId = 1,
                Status = Enums.OrderStatus.InProgress
            };
        }

        [HttpPut("Status")]
        public ActionResult UpdateOrderStatus(OrderStatus status)
        {
            return Ok("Order status is successfully updated");
        }
    }
}