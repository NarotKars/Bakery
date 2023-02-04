using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlobController : ControllerBase
    {
        const string baseUrl = "https://bakery9.blob.core.windows.net";
        public BlobController()
        {

        }

        [HttpGet("{container}/{blobName}")]
        public async Task<Stream> GetBlobAsync(string container, string blobName)
        {
            var client = new Client();
            var url = baseUrl + $"/{container}/{blobName}";
            return await client.GetStreamAsync(HttpMethod.Get, string.Format(url, container, blobName));
        }
    }
}
