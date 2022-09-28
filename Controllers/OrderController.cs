using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetStoreApi.Domain;
using PetStoreApi.DTO.OrderDTO;
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
        [Authorize]
        public async Task<IActionResult> GetOrderList(int orderStatus = 0) {


            AppServiceResult<List<OrderDto>> result = await _orderRepository.GetListAllOrder(orderStatus);

            return Ok(result);
	    }
        [HttpPut("confirm")]
        [Authorize]
        public async Task<IActionResult> ConfirmOrder(Guid orderTrackingNumber) {
		    AppBaseResult result = await _orderRepository.UpdateOrderStatus(orderTrackingNumber, 3);

            return Ok(result);
	    }
        [HttpPut("cancel")]
        [Authorize]
        public async Task<IActionResult> CancelOrder(Guid orderTrackingNumber) {
            AppBaseResult result = await _orderRepository.UpdateOrderStatus(orderTrackingNumber, 5);

            return Ok(result);
        }
        [HttpDelete]
        [Authorize]
        public IActionResult DeleteOrder(Guid orderId) {
            AppBaseResult result = _orderRepository.DeleteOrder(orderId);

            return Ok(result);
        }
    }
}
