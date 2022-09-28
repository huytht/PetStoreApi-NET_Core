using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.OrderItemDTO;
using PetStoreApi.DTO.OrderStatusDTO;

namespace PetStoreApi.DTO.OrderDTO
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public double TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public string Reciever { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatusDto OrderStatus { get; set; }
        public int PaymentId { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = new HashSet<OrderItemDto>();

        public static OrderDto CreateFromEntity (Order src) 
        {
            OrderDto dto = new OrderDto();

            dto.Id = src.Id;
            dto.OrderDate = src.OrderDate;
            dto.TotalPrice = src.TotalPrice;
            dto.TotalQuantity = src.TotalQuantity;
            dto.OrderStatus = OrderStatusDto.CreateFromEntity(src.OrderStatus);
            dto.PaymentId = src.PaymentId;
            
            if (src.OrderItems.Count > 0)
            {
                foreach (OrderItem orderItem in src.OrderItems)
                {
                    dto.OrderItems.Add(OrderItemDto.CreateFromEntity(orderItem));
                }
            }

            dto.Address = src.Address;

            return dto;
        }

    }
}
