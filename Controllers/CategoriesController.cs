using Microsoft.AspNetCore.Mvc;
using OnlineStore.Exceptions;
using OnlineStore.Models;
using OnlineStore.Repositories;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoriesService categoriesService;
        public CategoriesController(CategoriesService categoriesRepository)
        {
            this.categoriesService = categoriesRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await this.categoriesService.GetCategoriesAsync();
        }
    }
}
