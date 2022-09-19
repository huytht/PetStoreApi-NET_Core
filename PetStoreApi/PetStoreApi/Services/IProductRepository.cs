using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.ProductDTO;
using PetStoreApi.Domain;

namespace PetStoreApi.Services
{
    public interface IProductRepository
    {
        //IEnumerable<Product> GetDogList();
        //IEnumerable<Product> GetCatList();
        //IEnumerable<Product> GetProductList();
        //Product GetProductById(int id);
        Task<AppServiceResult<ProductDto>> AddProduct(ProductCreateDto product);
    }
}
