using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UsersService usersService;
        public UsersController(UsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpPost("register")]
        public async Task<int> Register(User user)
        {
            return await this.usersService.Register(user);
        }

        [HttpPost("login")]
        public async Task<int> Login(User user)
        {
            return await this.usersService.Login(user);
        }
    }
}
