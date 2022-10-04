using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.ProductDTO;
using PetStoreApi.Domain;
using java.lang;

namespace PetStoreApi.Services
{
    public interface IProductRepository
    {
        Task<AppServiceResult<PaginatedList<ProductShortDto>>> GetProductList(PageParam pageParam, string type = "all");
        Task<AppServiceResult<PaginatedList<ProductShortDto>>> GetProductFilterList(PageParam pageParam, FilterParam filterParam);
        Task<AppServiceResult<PaginatedList<ProductShortDto>>> SearchProduct(PageParam pageParam, string keyword);
        AppServiceResult<ProductDto> GetProductById(Guid id);
        AppServiceResult<Product> AddProduct(ProductCreateDto product);
        Task<AppServiceResult<PaginatedList<ProductShortDto>>> GetWishList(PageParam pageParam);
        Task<AppBaseResult> UpdateWishList(Guid productId);
    }
}
