using PetStoreApi.Domain;
using PetStoreApi.DTO.OrderDTO;

namespace PetStoreApi.Services
{
    public interface IOrderRepository
    {
        Task<AppServiceResult<PaginatedList<OrderDto>>> GetListOrder(int orderStatus, PageParam pageParam);

        Task<AppServiceResult<List<OrderDto>>> GetListAllOrder(int orderStatus);

        Task<AppBaseResult> UpdateOrderStatus(Guid orderId, int orderStatusId);

        AppBaseResult DeleteOrder(Guid id);
    }
}
