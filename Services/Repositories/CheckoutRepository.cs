using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetStoreApi.Data.Entity;
using PetStoreApi.Domain;
using PetStoreApi.DTO.OrderDTO;
using PetStoreApi.DTO.OrderItemDTO;
using PetStoreApi.DTO.PurchaseDTO;
using PetStoreApi.Helpers;
using System;
using System.Security.Claims;

namespace PetStoreApi.Services.Repositories
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<OrderRepository> _logger;
        private readonly IHttpContextAccessor? _httpContextAccessor;
        public CheckoutRepository(DataContext context, ILogger<OrderRepository> logger, IHttpContextAccessor? httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AppServiceResult<PurchaseResponse>> PlaceOrder(PurchaseDto purchase)
        {
            try
            {
                string currentUsername = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                AppUser appUser = await _context.AppUsers.FirstOrDefaultAsync(u => u.Username.Equals(currentUsername));

                if (appUser == null)
                {
                    _logger.LogWarning("Not logged in!");

                    return new AppServiceResult<PurchaseResponse>(false, 101, "Not logged in!", null);
                }

                // retrieve the order into from DTO
                OrderCheckout orderCheckout = purchase.Order;
                Order order = new Order();

                // generate tracking number using UUID 
                Guid orderTrackingNumber = Guid.NewGuid();
                order.Id = orderTrackingNumber;
                order.OrderStatus = await _context.OrderStatuses.FirstOrDefaultAsync(os => os.Id == orderCheckout.OrderStatusId);
                order.Payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == orderCheckout.PaymentId);
                order.OrderDate = DateTime.UtcNow;
                order.TotalPrice = orderCheckout.TotalPrice;
                order.TotalQuantity = orderCheckout.TotalQuantity;
                order.Receiver = orderCheckout.Receiver;
                order.Phone = orderCheckout.Phone;

                // get address
                order.Address = orderCheckout.Address;

                // get customer info
                order.AppUser = appUser;

                await _context.Orders.AddAsync(order); 

                // check status of each product
                foreach (OrderItemDto item in purchase.OrderItems)
                {
                    Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                    
                    if (product != null)
                    {
                        if (product.Status == false)
                        {
                            _logger.LogWarning("product: " + product.Id + " is out of stock!");

                            return new AppServiceResult<PurchaseResponse>(false, 101, "Sản phẩm: " + product.Name + " đã hết hàng!", null);
                        }
                        else if (product.AmountInStock - item.Quantity < 0)
                        {
                            _logger.LogWarning("product: " + product.Id + " is exceed the quantity in stock!");

                            return new AppServiceResult<PurchaseResponse>(false, 101, "Sản phẩm: " + product.Name + " có số lượng vượt quá số lượng tồn!", null);
                        }
                        else
                        {
                            OrderItem orderItem = new OrderItem(item.ProductId, order.Id, item.Quantity, item.Price);
                            await _context.OrderItems.AddAsync(orderItem);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("product: " + item.ProductId + " is not exist!");

                        return new AppServiceResult<PurchaseResponse>(false, 101, "Sản phẩm: " + item.ProductId + " không tồn tại!", null);
                    }
                }

                await _context.SaveChangesAsync();

                return new AppServiceResult<PurchaseResponse>(true, 0, "Succeed!", new PurchaseResponse(orderTrackingNumber, order.OrderDate));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<PurchaseResponse>(false, 99, "Unknown", null);
            }
        }
    }
}
