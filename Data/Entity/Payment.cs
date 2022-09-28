namespace PetStoreApi.Data.Entity
{
    public class Payment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Order> Orders { get; set; }
        public Payment()
        {
            Orders = new HashSet<Order>();
        }
    }
}
