using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetStoreApi.Controllers;
using PetStoreApi.Data.Entity;
using PetStoreApi.Domain;
using PetStoreApi.DTO.OrderDTO;
using PetStoreApi.Helpers;
using System.Security.Claims;

namespace PetStoreApi.Services.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<OrderRepository> _logger;
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public OrderRepository(DataContext context, ILogger<OrderRepository> logger, IHttpContextAccessor? httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public AppBaseResult DeleteOrder(Guid id)
        {
            try
            {
                var order = _context.Orders.FirstOrDefault(o => o.Id == id);

                if (order != null)
                {
                    _context.Orders.Remove(order);
                    return AppBaseResult.GenarateIsSucceed();
                } else
                {
                    _logger.LogWarning("Order is not exist" + id + ", Cannot further process!");
                    return AppBaseResult.GenarateIsFailed(99, "Order is not exist: " + id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return AppBaseResult.GenarateIsFailed(99, "Unknown");
            }
        }

        public async Task<AppServiceResult<List<OrderDto>>> GetListAllOrder(int orderStatus)
        {
            try
            {
                IEnumerable<Order> list;

                if (orderStatus == 0)
                {
                    list = _context.Orders.Include("Payment").Include("OrderStatus").Include("OrderItems").Include("OrderItems.Product").Include("OrderItems.Product.ProductImages").OrderByDescending(o => o.OrderDate);
                }
                else
                {
                    list = _context.Orders.Include("Payment").Include("OrderStatus").Include("OrderItems").Include("OrderItems.Product").Include("OrderItems.Product.ProductImages").Where(o => o.OrderStatusId == orderStatus).OrderByDescending(o => o.OrderDate);
                }

                var result = list.Select(o => OrderDto.CreateFromEntity(o)).ToList();

                return new AppServiceResult<List<OrderDto>>(true, 0, "Success!", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new AppServiceResult<List<OrderDto>>(false, 99, "Unknown", null);
            }
        }

        public async Task<AppServiceResult<PaginatedList<OrderDto>>> GetListOrder(int orderStatus, PageParam pageParam)
        {
            try
            {
                var currentUsername = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var appUser = await _context.AppUsers.FirstOrDefaultAsync(u => u.Username.Equals(currentUsername));
                

                if (appUser == null)
                {
                    _logger.LogWarning("Not logged in!");
                    return new AppServiceResult<PaginatedList<OrderDto>>(false, 101, "Not logged in!", null);
                }

                IEnumerable<Order> list;

                if (orderStatus == 0)
                {
                    list = _context.Orders.Include("Payment").Include("OrderStatus").Include("OrderItems").Include("OrderItems.Product").Include("OrderItems.Product.ProductImages").Where(o => o.UserId == appUser.Id).OrderByDescending(o => o.OrderDate);
                }
                else
                {
                    list = _context.Orders.Include("Payment").Include("OrderStatus").Include("OrderItems").Include("OrderItems.Product").Include("OrderItems.Product.ProductImages").Where(o => o.UserId == appUser.Id && o.OrderStatusId == orderStatus).OrderByDescending(o => o.OrderDate);
                }

                var result = list.Select(o => OrderDto.CreateFromEntity(o)).ToList();

                PaginatedList<OrderDto> resultList = new PaginatedList<OrderDto>(result, pageParam.PageIndex, pageParam.PageSize);

                return new AppServiceResult<PaginatedList<OrderDto>>(true, 0, "Success!", resultList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new AppServiceResult<PaginatedList<OrderDto>>(false, 99, "Unknown", null);
            }
        }

        public async Task<AppBaseResult> UpdateOrderStatus(Guid orderId, int orderStatusId)
        {
            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    _logger.LogWarning("Order id is not exist!");

                    return AppBaseResult.GenarateIsFailed(101, "Order id is not exist!");
                }

                var orderStatus = await _context.OrderStatuses.FirstOrDefaultAsync(os => os.Id == orderStatusId);

                if (orderStatus == null)
                {
                    _logger.LogWarning("Order status id is not exist!");

                    return AppBaseResult.GenarateIsFailed(101, "Order status id is not exist!");
                }

                order.OrderStatus = orderStatus;

                await _context.SaveChangesAsync();

                return AppBaseResult.GenarateIsSucceed();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return AppBaseResult.GenarateIsFailed(99, "Unknown");
            }
        }
    }
}
