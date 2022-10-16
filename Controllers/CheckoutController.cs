using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetStoreApi.Domain;
using PetStoreApi.DTO.BreedDTO;
using PetStoreApi.DTO.PurchaseDTO;
using PetStoreApi.DTO.ResponseDTO;
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
        [Authorize(Roles = "ROLE_MEMBER, ROLE_ADMIN")]
        public async Task<IActionResult> PlaceOrder(PurchaseDto purchase)
        {
            AppServiceResult<PurchaseResponse> result = await _checkoutRepository.PlaceOrder(purchase);

            return result.success ? Ok(new HttpResponseSuccess<PurchaseResponse>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }
    }
}
