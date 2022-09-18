namespace PetStoreApi.Data.Entity
{
    public class ProductOrigin
    {
        public Guid ProductId { get; set; }
        public int OriginId { get; set; }

        public Product Product { get; set; }
        public Origin Origin { get; set; }
    }
}
