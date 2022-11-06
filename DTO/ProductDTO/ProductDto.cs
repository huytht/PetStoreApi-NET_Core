using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.BreedDTO;
using PetStoreApi.DTO.CategoryDTO;
using PetStoreApi.DTO.OriginDTO;
using static com.sun.tools.@internal.xjc.reader.xmlschema.bindinfo.BIConversion;

namespace PetStoreApi.DTO.ProductDTO
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public int AmountInStock { get; set; }
        public ICollection<OriginDto> Origins { get; set; } = new HashSet<OriginDto>();
        public ICollection<string> ImagePath { get; set; } = new HashSet<string>();
        public ICollection<ProductShortDto> ProductSuggestions { get; set; } = new HashSet<ProductShortDto>();
        public string? Description { get; set; }
        public bool? Gender { get; set; }
        public bool? Favourite { get; set; }
        public int? Rate { get; set; }
        public double Price { get; set; }
        public bool? Status { get; set; }
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
            dto.Gender = src.Gender;
            dto.Rate = src.Rate;
            dto.Price = src.Price;
            dto.Favourite = false;
            if (src.Category != null)
                dto.Category = CategoryDto.CreateFromEntity(src.Category);
            if (src.Age != null)
                dto.Age = src.Age;
            if (src.Gender != null)
                dto.Gender = src.Gender;
            if (src.Breed != null)
                dto.Breed = BreedDto.CreateFromEntity(src.Breed);
            if (src.ProductOrigins.Count > 0)
            {
                foreach (var productOrigin in src.ProductOrigins)
                {
                    if (productOrigin != null)
                    {
                        dto.Origins.Add(OriginDto.CreateFromEntity(productOrigin.Origin));
                    }
                }
            }
            if (src.ProductImages.Count > 0)
            {
                foreach (var image in src.ProductImages)
                {
                    dto.ImagePath.Add(image.ImagePath);
                }
            }
            return dto;
        }
        public static ProductDto CreateFromEntity(Product src, IEnumerable<ProductShortDto> suggestionList)
        {
            ProductDto dto = new ProductDto();

            dto.Id = src.Id;
            dto.Name = src.Name;
            dto.Age = src.Age;
            dto.AmountInStock = src.AmountInStock;
            dto.Description = src.Description;
            dto.Status = src.Status;
            dto.Price = src.Price;
            dto.Rate = src.Rate;
            dto.Favourite = false;
            dto.Gender = src.Gender;
            if (src.Category != null)
                dto.Category = CategoryDto.CreateFromEntity(src.Category);
            if (src.Age != null)
                dto.Age = src.Age;
            if (src.Gender != null)
                dto.Gender = src.Gender;
            if (src.Breed != null)
                dto.Breed = BreedDto.CreateFromEntity(src.Breed);
            if (src.ProductOrigins.Count > 0)
            {
                foreach (var productOrigin in src.ProductOrigins)
                {
                    if (productOrigin != null)
                    {
                        dto.Origins.Add(OriginDto.CreateFromEntity(productOrigin.Origin));
                    }
                }
            }
            if (src.ProductImages.Count > 0)
            {
                foreach (var image in src.ProductImages)
                {
                    dto.ImagePath.Add(image.ImagePath);
                }
            }
            if (suggestionList.Count() > 0)
            {
                foreach (var product in suggestionList)
                {
                    dto.ProductSuggestions.Add(product);
                }
            }

            return dto;
        }
        public static ProductDto CreateFromEntity(Product src, IEnumerable<ProductShortDto> suggestionList, Guid userId)
        {
            ProductDto dto = new ProductDto();

            dto.Id = src.Id;
            dto.Name = src.Name;
            dto.Age = src.Age;
            dto.AmountInStock = src.AmountInStock;
            dto.Description = src.Description;
            dto.Status = src.Status;
            dto.Price = src.Price;
            dto.Rate = src.Rate;
            dto.Gender = src.Gender;
            if (src.Category != null)
                dto.Category = CategoryDto.CreateFromEntity(src.Category);
            if (src.Age != null)
                dto.Age = src.Age;
            if (src.Gender != null)
                dto.Gender = src.Gender;
            if (src.Breed != null)
                dto.Breed = BreedDto.CreateFromEntity(src.Breed);
            if (src.ProductOrigins.Count > 0)
            {
                foreach (var productOrigin in src.ProductOrigins)
                {
                    if (productOrigin != null)
                    {
                        dto.Origins.Add(OriginDto.CreateFromEntity(productOrigin.Origin));
                    }
                }
            }
            if (src.AppUserProducts.Count > 0)
                dto.Favourite = src.AppUserProducts.FirstOrDefault(a => a.ProductId == src.Id && a.UserId == userId)?.Favourite;
            else
                dto.Favourite = false;
            if (src.ProductImages.Count > 0)
            {
                foreach (var image in src.ProductImages)
                {
                    dto.ImagePath.Add(image.ImagePath);
                }
            }
            if (suggestionList.Count() > 0)
            {
                foreach (var product in suggestionList)
                {
                    dto.ProductSuggestions.Add(product);
                }
            }

            return dto;
        }
    }
}
