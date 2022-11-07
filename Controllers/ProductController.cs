using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PetStoreApi.DTO.ProductDTO;
using PetStoreApi.Domain;
using PetStoreApi.Services;
using PetStoreApi.Data.Entity;
using PetStoreApi.Constants;
using Microsoft.AspNetCore.Authorization;
using PetStoreApi.DTO.ResponseDTO;

namespace PetStoreApi.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet("{productType?}/list")]
        public async Task<IActionResult> GetProductList(string productType = "all", int pageNumber = PaginationConstant.PAGE_NUMBER_DEFAULT, int pageSize = PaginationConstant.PAGE_SIZE_DEFAULT)
        {
            PageParam pageParam = new PageParam(pageNumber, pageSize);

            AppServiceResult<PaginatedList<ProductShortDto>> result = await _productRepository.GetProductList(pageParam, productType);

            return result.success ? Ok(new HttpResponseSuccess<PaginatedList<ProductShortDto>>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }
       
        [HttpGet("{id}")]
        public IActionResult GetProductDetail(Guid id)
        {
            AppServiceResult<ProductDto> result = _productRepository.GetProductById(id);

            return result.success ? Ok(new HttpResponseSuccess<ProductDto>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpGet("{categoryId}/{breedId}/list")]
        public async Task<IActionResult> GetProductFilterList(int pageNumber = PaginationConstant.PAGE_NUMBER_DEFAULT, int pageSize = PaginationConstant.PAGE_SIZE_DEFAULT,
                                                 int categoryId = AppConstant.CATEGORY_ID_DEFAULT, int breedId = AppConstant.BREED_ID_DEFAULT)
        {
            PageParam pageParam = new PageParam(pageNumber, pageSize);
            FilterParam filterParam = new FilterParam(breedId, categoryId);

            AppServiceResult<PaginatedList<ProductShortDto>> result = await _productRepository.GetProductFilterList(pageParam, filterParam);

            return result.success ? Ok(new HttpResponseSuccess<PaginatedList<ProductShortDto>>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpGet("search/{keyword}")]
        public async Task<IActionResult> SearchProduct(string keyword, int pageNumber = PaginationConstant.PAGE_NUMBER_DEFAULT, int pageSize = PaginationConstant.PAGE_SIZE_DEFAULT)
        {
            PageParam pageParam = new PageParam(pageNumber, pageSize);

            AppServiceResult<PaginatedList<ProductShortDto>> result = await _productRepository.SearchProduct(pageParam, keyword);

            return result.success ? Ok(new HttpResponseSuccess<PaginatedList<ProductShortDto>>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpPost]
        [Authorize(Roles = "ROLE_ADMIN")]
        public IActionResult AddProduct([FromForm] ProductCreateDto product)
        {
            AppServiceResult<Product> result = _productRepository.AddProduct(product);

            return result.success ? Ok(new HttpResponseSuccess<Product>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpGet("remark")]
        public IActionResult GetRemarkList(Guid productId, int pageNumber = PaginationConstant.PAGE_NUMBER_DEFAULT, int pageSize = PaginationConstant.PAGE_SIZE_DEFAULT)
        {
            PageParam pageParam = new PageParam(pageNumber, pageSize);

            AppServiceResult<PaginatedList<RemarkProductDto>> result = _productRepository.GetRemarkListByProduct(productId, pageParam);

            return result.success ? Ok(new HttpResponseSuccess<PaginatedList<RemarkProductDto>>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpPost("remark")]
        [Authorize(Roles = "ROLE_MEMBER, ROLE_ADMIN")]
        public async Task<IActionResult> SaveRemark(RemarkProductDto dto)
        {
            AppBaseResult result = await _productRepository.SaveRemark(dto);

            return result.success ? Ok(new HttpResponseSuccess<string>("Succeed!")) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpGet("wish-list")]
        [Authorize(Roles = "ROLE_MEMBER, ROLE_ADMIN")]
        public async Task<IActionResult> GetWishList(int pageNumber = PaginationConstant.PAGE_NUMBER_DEFAULT, int pageSize = PaginationConstant.PAGE_SIZE_DEFAULT)
        {
            PageParam pageParam = new PageParam(pageNumber, pageSize);

            AppServiceResult<PaginatedList<ProductShortDto>> result = await _productRepository.GetWishList(pageParam);

            return result.success ? Ok(new HttpResponseSuccess<PaginatedList<ProductShortDto>>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpPut("wish-list")]
        [Authorize(Roles = "ROLE_MEMBER, ROLE_ADMIN")]
        public async Task<IActionResult> UpdateWishList(Guid productId)
        {
            AppBaseResult result = await _productRepository.UpdateWishList(productId);

            return result.success ? Ok(new HttpResponseSuccess<string>("Succeed!")) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpGet("list/{productType}")]
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task<IActionResult> GetProductListByType(string productType)
        {
            var result = await _productRepository.GetProductListByType(productType);

            return result.success ? Ok(new HttpResponseSuccess<List<ProductDto>>(result.data)) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpPut("amount")]
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task<IActionResult> UpdateAmount(Guid productId, int amount)
        {
            var result = await _productRepository.UpdateAmountInInventory(productId, amount);

            return result.success ? Ok(new HttpResponseSuccess<string>("Succeed!")) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpPut]
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task<IActionResult> UpdateProduct(Guid productId, [FromForm] ProductUpdateDto productUpdate)
        {
            var result = await _productRepository.UpdateProduct(productId, productUpdate);

            return result.success ? Ok(new HttpResponseSuccess<string>("Succeed!")) : BadRequest(new HttpResponseError(null, result.message));
        }
        [HttpDelete]
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            var result = await _productRepository.DeleteProduct(productId);

            return result.success ? Ok(new HttpResponseSuccess<string>("Succeed!")) : BadRequest(new HttpResponseError(null, result.message));
        }
    }
}
