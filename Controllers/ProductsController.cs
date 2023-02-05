using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private List<Product> products = new List<Product>();
        public ProductsController()
        {
            products.Add(new Product()
            {
                Id = 1,
                Container = "cakes",
                BlobName = "CherryCake",
                CategoryId = 1,
                Price = 15000M,
                Description = "Cherry pistachio pound cake"
            });

            products.Add(new Product()
            {
                Id = 2,
                Container = "cakes",
                BlobName = "RaspberryCake",
                CategoryId = 1,
                Price = 20000M,
                Description = "Grain-free chocolate raspberry cake"
            });

            products.Add(new Product()
            {
                Id = 3,
                Container = "cakes",
                BlobName = "PavlovaDessert",
                CategoryId = 1,
                Price = 15000M,
                Description = "Pavlova dessert with fresh berries"
            });

            products.Add(new Product()
            {
                Id = 4,
                Container = "cookies",
                BlobName = "MagicalTahiniChocolateChipCookies",
                CategoryId = 2,
                Price = 1000M,
                Description = "Magical tahini chocolate chip cookies"
            });

            products.Add(new Product()
            {
                Id = 5,
                Container = "cheesecakes",
                BlobName = "BlackberryAndWhiteChocolateTart",
                CategoryId = 3,
                Price = 26000M,
                Description = "Blackberry and white chocolate tart"
            });

            products.Add(new Product()
            {
                Id = 6,
                Container = "cheesecakes",
                BlobName = "NewYorkCheesecake",
                CategoryId = 3,
                Price = 30000M,
                Description = "New York cheesecake"
            });
        }

        [HttpGet]
        public List<Product> GetProducts()
        {
            return products;
        }

        [HttpGet("id")]
        public List<Product> GetProductsById(int categoryId)
        {
            return this.products.Where(x => x.CategoryId == categoryId).ToList();
        }
    }
}
