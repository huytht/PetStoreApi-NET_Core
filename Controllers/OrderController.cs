using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetStoreApi.Domain;
using PetStoreApi.DTO.OrderDTO;
using PetStoreApi.DTO.PurchaseDTO;
using PetStoreApi.DTO.ResponseDTO;
using PetStoreApi.Services;
using System.Xml.Linq;

namespace PetStoreApi.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        [HttpGet]
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task<IActionResult> GetOrderList(int orderStatus = 0) {

            AppServiceResult<List<OrderDto>> result = await _orderRepository.GetListAllOrder(orderStatus);

            return result.success ? Ok(new HttpResponseSuccess<List<OrderDto>>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpPut("confirm")]
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task<IActionResult> ConfirmOrder(Guid orderTrackingNumber) {
		    AppBaseResult result = await _orderRepository.UpdateOrderStatus(orderTrackingNumber, 3);

            return result.success ? Ok(new HttpResponseSuccess<string>("Succeed!")) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpPut("cancel")]
        [Authorize(Roles = "ROLE_MEMBER, ROLE_ADMIN")]
        public async Task<IActionResult> CancelOrder(Guid orderTrackingNumber) {
            AppBaseResult result = await _orderRepository.UpdateOrderStatus(orderTrackingNumber, 5);

            return result.success ? Ok(new HttpResponseSuccess<string>("Succeed!")) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpDelete]
        [Authorize(Roles = "ROLE_ADMIN")]
        public IActionResult DeleteOrder(Guid orderId) {
            AppBaseResult result = _orderRepository.DeleteOrder(orderId);

            return result.success ? Ok(new HttpResponseSuccess<string>("Succeed!")) : BadRequest(new HttpResponseError(null, result.message));
        }
    }
}
