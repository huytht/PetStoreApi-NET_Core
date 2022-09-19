using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PetStoreApi.DTO.ProductDTO;
using PetStoreApi.Domain;
using PetStoreApi.Services;

namespace PetStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        public IActionResult AddProduct([FromForm] ProductCreateDto product)
        {
            Task<AppServiceResult<ProductDto>> result = _productRepository.AddProduct(product);
            Console.WriteLine(result);
            return Ok(result);
        }
    }
}
