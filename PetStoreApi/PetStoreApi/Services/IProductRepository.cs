using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.ProductDTO;
using PetStoreApi.Domain;

namespace PetStoreApi.Services
{
    public interface IProductRepository
    {
        AppServiceResult<List<ProductShortDto>> GetDogList();
        AppServiceResult<List<ProductShortDto>> GetCatList();
        //IEnumerable<Product> GetProductList();
        //Product GetProductById(int id);
        AppServiceResult<Product> AddProduct(ProductCreateDto product);
    }
}
