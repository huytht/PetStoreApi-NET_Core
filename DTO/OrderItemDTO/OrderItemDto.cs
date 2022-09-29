using PetStoreApi.Data.Entity;

namespace PetStoreApi.DTO.OrderItemDTO
{
    public class OrderItemDto
    {
        public double Price { get; set; }
        public string? ImagePath { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public static OrderItemDto CreateFromEntity(OrderItem src) 
        {
            OrderItemDto dto = new OrderItemDto();

            dto.Price = src.Price;
            dto.Quantity = src.Quantity;

            if (src.Product != null)
            {
                dto.Name = src.Product.Name;
                dto.ImagePath = src.Product.ProductImages.First(e => e.ProductId == src.Product.Id).ImagePath;
            }

            return dto;
        }
    }
}
