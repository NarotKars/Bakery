using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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
        public async Task<IEnumerable<Product>> GetProductsByCategoryId(string id)
        {
            return await this.productsService.GetProductsByCategoryId(id);
        }

        [HttpGet("{id}")]
        public async Task<Product> GetProductById(string id)
        {
            return (await this.productsService.GetProductsByIds(new List<string>() { id })).First();
        }

        [HttpPost("Add")]
        public async Task<Product> AddProduct(ProductUploadParams productUploadParams)
        {
            return await this.productsService.AddProduct(productUploadParams);
        }

        [HttpDelete("{id}")]
        public async Task<Product> DeleteProduct(string id)
        {
            return await this.productsService.DeleteProduct(id);
        }

        [HttpPut("{id}")]
        public async Task UpdateProduct(string id, ProductUploadParams productUploadParams)
        {
            await this.productsService.UpdateProduct(id, productUploadParams);
        }
    }
}
