using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetStoreApi.Domain;
using PetStoreApi.DTO.PurchaseDTO;
using PetStoreApi.Services;

namespace PetStoreApi.Controllers
{
    [ApiController]
    [Route("api/checkout")]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutRepository _checkoutRepository;

        public CheckoutController(ICheckoutRepository checkoutRepository)
        {
            _checkoutRepository = checkoutRepository;
        }
        [HttpPost("purchase")]
        [Authorize]
        public async Task<IActionResult> PlaceOrder(PurchaseDto purchase)
        {
            AppServiceResult<PurchaseResponse> result = await _checkoutRepository.PlaceOrder(purchase);

            return Ok(result);
        }
    }
}
