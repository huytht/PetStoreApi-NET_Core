using PetStoreApi.Data.Entity;

namespace PetStoreApi.DTO.OrderStatusDTO
{
    public class OrderStatusDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public OrderStatusDto()
        {
        }
        public static OrderStatusDto CreateFromEntity(OrderStatus src)
        {
            OrderStatusDto dto = new OrderStatusDto();

            dto.Id = src.Id;
            dto.Name = src.Name;

            return dto;
        }
    }
}
