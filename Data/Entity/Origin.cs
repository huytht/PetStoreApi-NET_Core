namespace PetStoreApi.Data.Entity
{
    public class Origin
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ProductOrigin> ProductOrigins { get; set; }

        public Origin()
        {
            ProductOrigins = new HashSet<ProductOrigin>();
        }
    }
}
