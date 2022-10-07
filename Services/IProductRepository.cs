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
        AppServiceResult<Product> AddProduct(ProductCreateDto product);
        Task<AppServiceResult<PaginatedList<ProductShortDto>>> GetWishList(PageParam pageParam);
        Task<AppBaseResult> UpdateWishList(Guid productId);
        Task<AppBaseResult> SaveRemark(RemarkProductDto remarkProduct);
        AppServiceResult<PaginatedList<RemarkProductDto>> GetRemarkListByProduct(Guid productId, PageParam pageParam);
        Task<AppServiceResult<List<ProductDto>>> GetProductListByType(string type = "");
        Task<AppBaseResult> UpdateAmountInInventory(Guid productId, int amount);
        Task<AppBaseResult> UpdateProduct(Guid productId, ProductUpdateDto product);
        Task<AppBaseResult> DeleteProduct(Guid productId);
    }
}
