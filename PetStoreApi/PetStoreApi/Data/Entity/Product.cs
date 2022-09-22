using PetStoreApi.DTO.ProductDTO;
using System.Numerics;
using ToStringSourceGenerator.Attributes;

namespace PetStoreApi.Data.Entity
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int AmountInStock { get; set; }
        public int? Age { get; set; }
        public string? Description { get; set; }
        public bool? Gender { get; set; }
        public bool? Status { get; set; }
        public double Price { get; set; }
        public int? Rate { get; set; }
        public int? BreedId { get; set; }
        public int CategoryId { get; set; }
        public Breed? Breed { get; set; }
        public Category Category { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<ProductOrigin> ProductOrigins { get; set; }
        public Product()
        {
            OrderItems = new HashSet<OrderItem>();
            ProductImages = new HashSet<ProductImage>();
            ProductOrigins = new HashSet<ProductOrigin>();
        }
    }
}
