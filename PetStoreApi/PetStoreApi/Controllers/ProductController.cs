using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PetStoreApi.DTO.ProductDTO;
using PetStoreApi.Domain;
using PetStoreApi.Services;
using PetStoreApi.Data.Entity;

namespace PetStoreApi.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet("cat/list")]
        public IActionResult GetCatList()
        {
            AppServiceResult<List<ProductShortDto>> result = _productRepository.GetCatList();

            return Ok(result);
        }
        [HttpGet("dog/list")]
        public IActionResult GetDogList()
        {
            AppServiceResult<List<ProductShortDto>> result = _productRepository.GetDogList();

            return Ok(result);
        }
        [HttpPost]
        public IActionResult AddProduct([FromForm] ProductCreateDto product)
        {
            AppServiceResult<Product> result = _productRepository.AddProduct(product);
            Console.WriteLine("=========================================>" + result);
            return Ok(result);
        }
    }
}
