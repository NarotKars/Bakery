using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
