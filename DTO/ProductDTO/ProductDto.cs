﻿using PetStoreApi.Data.Entity;
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
        public ICollection<OriginDto> Origins { get; set; } = new HashSet<OriginDto>();
        public ICollection<string> ImagePath { get; set; } = new HashSet<string>();
        public ICollection<ProductShortDto> ProductSuggestions { get; set; } = new HashSet<ProductShortDto>();
        public string? Description { get; set; }
        public bool? Gender { get; set; }
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
            dto.Price = src.Price;
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
    }
}