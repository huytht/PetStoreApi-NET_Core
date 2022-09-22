using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.ProductDTO;
using PetStoreApi.Domain;

namespace PetStoreApi.Services
{
    public interface IProductRepository
    {
        AppServiceResult<PaginatedList<ProductShortDto>> GetProductList(PageParam pageParam, string type = "all");
        AppServiceResult<PaginatedList<ProductShortDto>> GetProductFilterList(PageParam pageParam, FilterParam filterParam);
        AppServiceResult<PaginatedList<ProductShortDto>> SearchProduct(PageParam pageParam, string keyword);
        AppServiceResult<ProductDto> GetProductById(Guid id);
        //IEnumerable<Product> GetProductList();
        //Product GetProductById(int id);
        AppServiceResult<Product> AddProduct(ProductCreateDto product);
    }
}
