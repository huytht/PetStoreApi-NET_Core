namespace PetStoreApi.Data.Entity
{
    public class OrderItem
    {
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }

        public OrderItem(Guid productId, Guid orderId, int quantity, double price)
        {
            ProductId = productId;
            OrderId = orderId;
            Quantity = quantity;
            Price = price;
        }
    }
}
