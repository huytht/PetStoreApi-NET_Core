using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.OriginDTO;

namespace PetStoreApi.DTO.ProductDTO
{
    public class ProductOriginDto
    {
        public ProductDto Product { get; set; }
        public OriginDto Origin { get; set; }

        public ProductOriginDto()
        {
        }
        public static ProductOriginDto CreateFromEntity(ProductOrigin src)
        {
            ProductOriginDto dto = new ProductOriginDto();

            dto.Product = ProductDto.CreateFromEntity(src.Product);
            dto.Origin = OriginDto.CreateFromEntity(src.Origin);

            return dto;
                
        }
    }
}
