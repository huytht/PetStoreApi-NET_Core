namespace PetStoreApi.Data.Entity
{
    public class OrderStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Order> Orders { get; set; }
        public OrderStatus()
        {
            Orders = new HashSet<Order>();
        }
    }
}
