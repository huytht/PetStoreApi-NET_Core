using PetStoreApi.Controllers;
using PetStoreApi.Data.Entity;
using PetStoreApi.Helpers;

namespace PetStoreApi.Services.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly DataContext _context;

        public OrderItemRepository(DataContext context)
        {
            _context = context;
        }
    }
}
