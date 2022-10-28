using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.BreedDTO;
using PetStoreApi.DTO.CategoryDTO;
using PetStoreApi.DTO.OriginDTO;

namespace PetStoreApi.DTO.ProductDTO
{
    public class ProductShortDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int AmountInStock { get; set; }
        public string ImagePath { get; set; }
        public double Price { get; set; }
        public int? Rate { get; set; }
        public bool? Favourite { get; set; }
        public ProductShortDto()
        {
        }
        public static ProductShortDto CreateFromEntity(Product src)
        {
            ProductShortDto dto = new ProductShortDto();

            dto.Id = src.Id;
            dto.Name = src.Name;
            dto.AmountInStock = src.AmountInStock;
            dto.Price = src.Price;
            dto.Favourite = false;
            dto.Rate = src.Rate;
            if (src.ProductImages.Count > 0)
                dto.ImagePath = src.ProductImages.First(e => e.ProductId == src.Id).ImagePath;

            return dto;
        }

        public static ProductShortDto CreateFromEntity(Product src, Guid userId)
        {
            ProductShortDto dto = new ProductShortDto();

            dto.Id = src.Id;
            dto.Name = src.Name;
            dto.AmountInStock = src.AmountInStock;
            dto.Price = src.Price;
            if (src.AppUserProducts.Count > 0)
                dto.Favourite = src.AppUserProducts.FirstOrDefault(a => a.ProductId == src.Id && a.UserId == userId)?.Favourite;
            else
                dto.Favourite = false;
            dto.Rate = src.Rate;
            if (src.ProductImages.Count > 0)
                dto.ImagePath = src.ProductImages.First(e => e.ProductId == src.Id).ImagePath;

            return dto;
        }

    }
}
