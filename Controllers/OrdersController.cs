using Microsoft.AspNetCore.Mvc;
using OnlineStore.Enums;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersService ordersService;
        public OrdersController(OrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        [HttpPost("Create")]
        public async Task<int> CreateOrder(Order order)
        {
            return await this.ordersService.CreateOrder(order);
        }

        [HttpGet("{id}")]
        public async Task<Order> GetOrder(int id)
        {
            return await this.ordersService.GetOrder(id);
        }

        [HttpPut("Status")]
        public ActionResult UpdateOrderStatus(OrderStatus status)
        {
            return Ok("Order status is successfully updated");
        }
    }
}