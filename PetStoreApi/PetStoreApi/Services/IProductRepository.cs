using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.ProductDTO;
using PetStoreApi.Domain;

namespace PetStoreApi.Services
{
    public interface IProductRepository
    {
        Task<AppServiceResult<PaginatedList<ProductShortDto>>> GetProductList(PageParam pageParam, string type = "all");
        Task<AppServiceResult<PaginatedList<ProductShortDto>>> GetProductFilterList(PageParam pageParam, FilterParam filterParam);
        Task<AppServiceResult<PaginatedList<ProductShortDto>>> SearchProduct(PageParam pageParam, string keyword);
        AppServiceResult<ProductDto> GetProductById(Guid id);
        //IEnumerable<Product> GetProductList();
        //Product GetProductById(int id);
        AppServiceResult<Product> AddProduct(ProductCreateDto product);
    }
}
