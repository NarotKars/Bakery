namespace OnlineStore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Container { get; set; }
        public string BlobName { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
