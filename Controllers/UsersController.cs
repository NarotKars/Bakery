using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        public UsersController()
        {

        }

        [HttpPost("Login")]
        public int Login(User user)
        {
            //returns the id of the user
            return 1;
        }
    }
}
