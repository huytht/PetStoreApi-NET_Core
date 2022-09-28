using PetStoreApi.Data.Entity;

namespace PetStoreApi.Services
{
    public interface IOrderItemRepository
    {
        public void AddOrderedProducts(OrderItem orderItem);
    }
}
