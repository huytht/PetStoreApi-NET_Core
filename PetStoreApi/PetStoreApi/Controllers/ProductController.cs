using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PetStoreApi.DTO.ProductDTO;
using PetStoreApi.Domain;
using PetStoreApi.Services;
using PetStoreApi.Data.Entity;
using PetStoreApi.Constants;
using Microsoft.AspNetCore.Authorization;

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

            return Ok(result);
        }
        [HttpGet("{id}")]
        public IActionResult GetProductDetail(Guid id)
        {
            AppServiceResult<ProductDto> result = _productRepository.GetProductById(id);

            return Ok(result);
        }
        [HttpGet("{categoryId}/{breedId}/list")]
        public async Task<IActionResult> GetProductFilterList(int pageNumber = PaginationConstant.PAGE_NUMBER_DEFAULT, int pageSize = PaginationConstant.PAGE_SIZE_DEFAULT,
                                                 int categoryId = AppConstant.CATEGORY_ID_DEFAULT, int breedId = AppConstant.BREED_ID_DEFAULT)
        {
            PageParam pageParam = new PageParam(pageNumber, pageSize);
            FilterParam filterParam = new FilterParam(breedId, categoryId);

            AppServiceResult<PaginatedList<ProductShortDto>> result = await _productRepository.GetProductFilterList(pageParam, filterParam);

            return Ok(result);
        }
        [HttpGet("search/{keyword}")]
        public async Task<IActionResult> SearchProduct(string keyword, int pageNumber = PaginationConstant.PAGE_NUMBER_DEFAULT, int pageSize = PaginationConstant.PAGE_SIZE_DEFAULT)
        {
            PageParam pageParam = new PageParam(pageNumber, pageSize);

            AppServiceResult<PaginatedList<ProductShortDto>> result = await _productRepository.SearchProduct(pageParam, keyword);

            return Ok(result);
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddProduct([FromForm] ProductCreateDto product)
        {
            AppServiceResult<Product> result = _productRepository.AddProduct(product);
            return result.success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest();
        }
    }
}
