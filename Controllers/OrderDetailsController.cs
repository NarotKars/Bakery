using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailsController : ControllerBase
    {
        private readonly OrdersService ordersService;
        public OrderDetailsController(OrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        [HttpGet("{orderId}")]
        public async Task<List<OrderDetail>> GetOrderDetails(int orderId)
        {
            return await this.ordersService.GetOrderDetails(orderId);
        }

        [HttpPost("Add/{orderId}")]
        public async Task AddOrderDetail(int orderId, OrderDetail orderDetail)
        {
            await this.ordersService.AddOrderDetail(orderId, orderDetail, true);
        }

        [HttpDelete("Remove/{orderId}")]
        public ActionResult RemoveOrderDetail(int orderId, OrderDetail orderDetail)
        {
            return Ok("Your product is successfully removed from the order");
        }
    }
}
