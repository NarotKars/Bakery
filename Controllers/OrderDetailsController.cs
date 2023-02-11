using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailsController : ControllerBase
    {
        public OrderDetailsController()
        {

        }

        [HttpGet("{id}")]
        public List<OrderDetail> GetOrderDetails()
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            return orderDetails;
        }

        [HttpPost("Add/{orderId}")]
        public ActionResult AddOrderDetail(OrderDetail orderDetail)
        {
            return Ok("Your product is successfully added to the order");
        }

        [HttpDelete("Remove/{orderId}")]
        public ActionResult RemoveOrderDetail(OrderDetail orderDetail)
        {
            return Ok("Your product is successfully removed from the order");
        }
    }
}
