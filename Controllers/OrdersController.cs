using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
    }
}