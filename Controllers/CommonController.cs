using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PetStoreApi.Data.Entity;
using PetStoreApi.Domain;
using PetStoreApi.DTO.BreedDTO;
using PetStoreApi.DTO.CategoryDTO;
using PetStoreApi.DTO.OrderStatusDTO;
using PetStoreApi.DTO.OriginDTO;
using PetStoreApi.DTO.ResponseDTO;
using PetStoreApi.Services;
using System.IO;

namespace PetStoreApi.Controllers
{
    [ApiController]
    [Route("api/common")]
    public class CommonController : ControllerBase
    {
        private readonly ICommonRepository _commonRepository;
        public CommonController(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        [HttpGet("list/breed")]
        public async Task<IActionResult> GetListBreedByCategory(int categoryId = 0)
        {
            AppServiceResult<List<Breed?>> result = await _commonRepository.GetAllBreedByCategory(categoryId);

            return result.success ? Ok(new HttpResponseSuccess<List<Breed?>>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }

        [HttpGet("list/all/breed")]
        public async Task<IActionResult> GetListBreed()
        {
            AppServiceResult<List<BreedDto>> result = await _commonRepository.GetBreedList();

            return result.success ? Ok(new HttpResponseSuccess<List<BreedDto>>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }

        [HttpGet("list/origin")]
        public async Task<IActionResult> GetListOrigin()
        {
            AppServiceResult<List<OriginDto>> result = await _commonRepository.GetOriginList();

            return result.success ? Ok(new HttpResponseSuccess<List<OriginDto>>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }

        [HttpGet("list/category")]
        public async Task<IActionResult> GetListCategory()
        {
            AppServiceResult<List<CategoryDto>> result = await _commonRepository.GetCategoryList();

            return result.success ? Ok(new HttpResponseSuccess<List<CategoryDto>>(result.data)) : BadRequest(new HttpResponseError(null, result.message));

        }

        [HttpGet("list/order-status")]
        public async Task<IActionResult> getListOrderStatus()
        {
            AppServiceResult<List<OrderStatusDto>> result = await _commonRepository.GetOrderStatusList();

            return result.success ? Ok(new HttpResponseSuccess<List<OrderStatusDto>>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }
    }
}
