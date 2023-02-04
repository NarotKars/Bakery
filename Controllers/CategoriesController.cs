using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        public CategoriesController()
        {

        }

        [HttpGet]
        public List<Category> GetCategories()
        {
            return new List<Category>()
            {
                new Category() { Name = "Cakes", Id = 1},
                new Category() { Name = "Cookies", Id = 2},
                new Category() { Name = "Cheesecakes", Id = 3},
            };
        }
    }
}
