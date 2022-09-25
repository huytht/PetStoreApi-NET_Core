namespace PetStoreApi.Data.Entity
{
    public class ProductImage
    {
        public Guid Id { get; set; }
        public string ImagePath { get; set; }
        public Guid ProductId { get; set; }

        public Product Product { get; set; }
    }
}
