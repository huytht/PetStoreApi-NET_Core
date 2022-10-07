namespace PetStoreApi.DTO.ProductDTO
{
    public class ProductUpdateDto
    {
        public string Name { get; set; }
        public int AmountInStock { get; set; }
        public string Description { get; set; }
        public IFormFile?[]? ImageFiles { get; set; }
        public bool? Gender { get; set; }
        public double Price { get; set; }
        public int? Age { get; set; }
        public bool Status { get; set; }
        public int? BreedId { get; set; }
        public int?[]? OriginIds { get; set; }
        public int CategoryId { get; set; }

        public ProductUpdateDto()
        {
        }
        public ProductUpdateDto(string name, int age, int amountInStock, string description, IFormFile?[]? imageFiles, bool gender, double price, bool status, int breedId, int?[] originIds, int categoryId)
        {
            Name = name;
            Age = age;
            AmountInStock = amountInStock;
            Description = description;
            ImageFiles = imageFiles;
            Gender = gender;
            Price = price;
            Status = status;
            BreedId = breedId;
            OriginIds = originIds;
            CategoryId = categoryId;
        }
        public ProductUpdateDto(string name, int amountInStock, string description, IFormFile?[]? imageFiles, double price, bool status, int categoryId)
        {
            Name = name;
            AmountInStock = amountInStock;
            Description = description;
            ImageFiles = imageFiles;
            Price = price;
            Status = status;
            CategoryId = categoryId;
        }
    }
}
