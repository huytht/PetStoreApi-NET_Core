using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.BreedDTO;
using PetStoreApi.DTO.CategoryDTO;
using PetStoreApi.DTO.OriginDTO;

namespace PetStoreApi.DTO.ProductDTO
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public int AmountInStock { get; set; }
        public ICollection<ProductOriginDto> ProductOrigins { get; set; }
        public ICollection<string> ImagePath  { get; set; }
        public string Description { get; set; }
        public bool? Gender { get; set; }
        public double Price { get; set; }
        public bool Status { get; set; }
        public BreedDto Breed { get; set; }
        public CategoryDto Category { get; set; }

        public ProductDto()
        {
        }
        public static ProductDto CreateFromEntity(Product src)
        {
            ProductDto dto = new ProductDto();

            dto.Id = src.Id;
            dto.Name = src.Name;
            dto.Age = src.Age;
            dto.AmountInStock = src.AmountInStock;
            dto.Description = src.Description;
            dto.Status = src.Status;
            dto.Price = src.Price;
            dto.Category = CategoryDto.CreateFromEntity(src.Category);
            if (src.Age != null)
                dto.Age = src.Age;
            if (src.Gender != null)
                dto.Gender = src.Gender;
            if (src.Breed != null)
                dto.Breed = BreedDto.CreateFromEntity(src.Breed);
            if (src.ProductOrigins.Count > 0)
            {
                foreach(var origin in src.ProductOrigins)
                {
                    dto.ProductOrigins.Add(ProductOriginDto.CreateFromEntity(origin));
                }
            }
            if (src.ProductImages.Count > 0)
            {
                foreach(var image in src.ProductImages)
                {
                    dto.ImagePath.Add(image.ImagePath);
                }
            }

            return dto;
        }

    }
}
