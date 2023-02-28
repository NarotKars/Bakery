using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OnlineStore.Exceptions;
using OnlineStore.Models;
using OnlineStore.Services;

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
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await this.categoriesService.GetCategories();
        }
        
        [HttpGet("{id}")]
        public async Task<Category> GetCategoriesById(string id)
        {
            return await this.categoriesService.GetCategoryById(id);
        }
    }
}
