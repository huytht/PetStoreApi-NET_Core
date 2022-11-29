using PetStoreApi.DTO.OrderStatusDTO;

namespace PetStoreApi.DTO.OrderDTO
{
    public class OrderCheckout
    {
        public double TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public string Receiver { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int OrderStatusId { get; set; }
        public int PaymentId { get; set; }
    }
}
