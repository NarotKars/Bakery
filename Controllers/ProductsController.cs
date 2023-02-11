using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Services;
using System.Collections;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsService productsService;

        public ProductsController(ProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await this.productsService.GetProducts();
        }

        [HttpGet("categoryId")]
        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId)
        {
            return await this.productsService.GetProductsByCategoryId(categoryId);
        }
    }
}
