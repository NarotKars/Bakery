using System.Data;
using OnlineStore.Models;
using Dapper;
using OnlineStore.Exceptions;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;

namespace OnlineStore.Services
{
    public class ProductsService
    {
        private readonly IConfiguration configuration;
        private readonly BlobService blobService;

        public ProductsService(IConfiguration configuration, BlobService blobService)
        {
            this.configuration = configuration;
            this.blobService = blobService;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            string sql = "GetProducts";
            return await ConnectionManager.CreateConnection(configuration)
                                          .QueryAsync<Product>(sql, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId)
        {
            string sql = "GetProducts";
            return await ConnectionManager.CreateConnection(configuration)
                                          .QueryAsync<Product>(sql, new { CategoryId = categoryId }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Product>> GetProductsByIds(string ids, SqlConnection connection = null, DbTransaction transaction = null)
        {
            string sql = "GetProducts";
            if(connection == null)
            {
                connection = ConnectionManager.CreateConnection(configuration);
            }
            var product = await connection.QueryAsync<Product>(sql, new { ProductIds = ids }, transaction, commandType: CommandType.StoredProcedure);
            if(product == default || !product.Any())
            {
                throw new RESTException("The specified product(s) are not found", System.Net.HttpStatusCode.NotFound);
            }
            return product;

        }

        public async Task<Product> AddProduct(ProductUploadParams productUploadParams)
        {
            await this.blobService.UploadToBlob(productUploadParams.LocalFilePath, productUploadParams.Container, productUploadParams.BlobName);
            string sql = "CreateProduct";
            var parameters = new DynamicParameters();
            parameters.Add("Container", productUploadParams.Container);
            parameters.Add("BlobName", productUploadParams.BlobName);
            parameters.Add("CategoryId", productUploadParams.CategoryId);
            parameters.Add("Description", productUploadParams.Description);
            parameters.Add("Price", productUploadParams.Price);
            parameters.Add("Id", null, DbType.Int32, ParameterDirection.Output);
            await ConnectionManager.CreateConnection(configuration).ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
            return new Product()
            {
                Id = parameters.Get<int>("Id"),
                Container = productUploadParams.Container,
                BlobName = productUploadParams.BlobName,
                CategoryId = productUploadParams.CategoryId,
                Description = productUploadParams.Description,
                Price = productUploadParams.Price
            };
        }

        public async Task<Product> DeleteProduct(int id)
        {
            var product = (await this.GetProductsByIds(id.ToString())).FirstOrDefault();
            if(product == default)
            {
                throw new RESTException("There is no product with the specified id", System.Net.HttpStatusCode.NotFound);
            }
            await this.blobService.DeleteFromBlob(product.Container, product.BlobName);
            var sql = "DeleteProduct";
            await ConnectionManager.CreateConnection(configuration).ExecuteAsync(sql, new { Id = id }, commandType: CommandType.StoredProcedure);
            return product;
        }

        public async Task<Product> UpdateProduct(int id, ProductUploadParams productUploadParams)
        {
            var product = (await this.GetProductsByIds(id.ToString())).FirstOrDefault();
            if (product == default)
            {
                throw new RESTException("There is no product with the specified id", System.Net.HttpStatusCode.NotFound);
            }
            await this.blobService.DeleteFromBlob(product.Container, product.BlobName);
            await this.blobService.UploadToBlob(productUploadParams.LocalFilePath, productUploadParams.Container, productUploadParams.BlobName);

            string sql = "UpdateProduct";
            var parameters = new DynamicParameters();
            parameters.Add("Container", productUploadParams.Container);
            parameters.Add("BlobName", productUploadParams.BlobName);
            parameters.Add("CategoryId", productUploadParams.CategoryId);
            parameters.Add("Description", productUploadParams.Description);
            parameters.Add("Price", productUploadParams.Price);
            parameters.Add("Id", id);
            await ConnectionManager.CreateConnection(configuration).ExecuteAsync(sql, parameters , commandType: CommandType.StoredProcedure);
            return new Product()
            {
                Id = id,
                Container = productUploadParams.Container,
                BlobName = productUploadParams.BlobName,
                CategoryId = productUploadParams.CategoryId,
                Description = productUploadParams.Description,
                Price = productUploadParams.Price
            };

        }
    }
}