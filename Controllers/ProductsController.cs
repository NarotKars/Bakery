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

        [HttpGet("ByCategory/{id}")]
        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int id)
        {
            return await this.productsService.GetProductsByCategoryId(id);
        }

        [HttpGet("{id}")]
        public async Task<Product> GetProductById(int id)
        {
            return (await this.productsService.GetProductsByIds(id.ToString())).First();
        }

        [HttpPost("Add")]
        public async Task<Product> AddProduct(ProductUploadParams productUploadParams)
        {
            return await this.productsService.AddProduct(productUploadParams);
        }

        [HttpDelete("{id}")]
        public async Task<Product> DeleteProduct(int id)
        {
            return await this.productsService.DeleteProduct(id);
        }

        [HttpPut("{id}")]
        public async Task<Product> UpdateProduct(int id, ProductUploadParams productUploadParams)
        {
            return await this.productsService.UpdateProduct(id, productUploadParams);
        }
    }
}
