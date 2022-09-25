namespace PetStoreApi.Data.Entity
{
    public class Breed
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
        public Breed()
        {
            Products = new HashSet<Product>();
        }
    }
}
