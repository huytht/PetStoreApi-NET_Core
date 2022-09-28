namespace PetStoreApi.Data.Entity
{
    public enum TinhTrangDonDatHang
    {
        New = 0, Paid = 1, Completed = 2, Canceled = -1
    }
    public class Order
    {
        public Guid Id { get; set; }
        public double TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public string Reciever { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid UserId { get; set; }
        public int PaymentId { get; set; }
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Payment Payment { get; set; }
        public AppUser AppUser { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }
    }
}
