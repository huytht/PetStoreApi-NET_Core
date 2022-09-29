using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.OrderDTO;
using PetStoreApi.DTO.OrderItemDTO;

namespace PetStoreApi.DTO.PurchaseDTO
{
    public class PurchaseDto
    {
        public AppUser? AppUser { get; set; }
        public OrderCheckout Order { get; set; }
        public ISet<OrderItemDto> OrderItems { get; set; }

        public PurchaseDto()
        {
            OrderItems = new HashSet<OrderItemDto>();
        }

    }
}
