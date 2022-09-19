using Microsoft.Extensions.Logging;
using PetStoreApi.Constants;
using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.ProductDTO;
using PetStoreApi.Domain;
using PetStoreApi.Helpers;
using System.Collections;

namespace PetStoreApi.Services.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        private readonly IBreedRepository _breedRepository;

        private readonly ICategoryRepository _categoryRepository;

        private readonly IOriginRepository _originRepository;

        private readonly IFileRepository _fileRepository;

        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(DataContext context, IBreedRepository breedRepository, ICategoryRepository categoryRepository, ILogger<ProductRepository> logger, IFileRepository fileRepository, IOriginRepository originRepository)
        {
            _context = context;
            _breedRepository = breedRepository;
            _categoryRepository = categoryRepository;
            _fileRepository = fileRepository;
            _logger = logger;
            _originRepository = originRepository;
        }

        public async Task<AppServiceResult<ProductDto>> AddProduct(ProductCreateDto product)
        {
            try
            {
                Product newProduct = new Product();
                newProduct.Name = product.Name;
                if (product.BreedId != null)
                {
                    Breed breed = await _breedRepository.GetBreed(product.BreedId);

                    if (breed != null)
                    {
                        newProduct.BreedId = breed.Id;
                    } else
                    {
                        _logger.LogWarning("Breed Id: " + product.BreedId + " is not exist!");
                        return null;
                    }
                }
                if (product.CategoryId != null)
                {
                    Category category = await _categoryRepository.GetCategory(product.CategoryId);

                    if (category != null)
                    {
                        newProduct.CategoryId = category.Id;
                    }
                    else
                    {
                        _logger.LogWarning("Category Id: " + product.CategoryId + " is not exist!");
                        return null;
                    }
                }
                if (product.OriginIds != null)
                {
                    foreach (var originId in product.OriginIds)
                    {
                        Origin origin = await _originRepository.GetOrigin(originId);

                        if (origin != null)
                        {
                            ProductOrigin productOrigin = new ProductOrigin();
                            productOrigin.Product = newProduct;
                            productOrigin.OriginId = origin.Id;
                            newProduct.ProductOrigins.Add(productOrigin);
                        }
                        else
                        {
                            _logger.LogWarning("Origin Id: " + originId + " is not exist!");
                            return null;
                        }
                    }
                   
                }
                if (product.Gender != null)
                {
                    newProduct.Gender = product.Gender;
                }
                if (product.Age != null)
                {
                    newProduct.Age = product.Age;
                }
                newProduct.Description = product.Description;
                newProduct.Price = product.Price;
                newProduct.AmountInStock = product.AmountInStock;
                newProduct.Status = product.Status;
                if (product.ImageFiles != null)
                {
                    foreach(IFormFile file in product.ImageFiles) 
                    {
                        string imagePath = _fileRepository.Upload(newProduct.Name, file);
                        ProductImage productImage = new ProductImage();
                        productImage.ImagePath = imagePath;
                        productImage.Product = newProduct;
                        newProduct.ProductImages.Add(productImage);
                    }
                }
                Console.WriteLine("===================>"+newProduct.ToString());
                _context.Add(newProduct);
                await _context.SaveChangesAsync();

                ProductDto dto = ProductDto.CreateFromEntity(newProduct);

                return new AppServiceResult<ProductDto>(true, 0, "Succeed!", dto);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<ProductDto>(false, 99, "Unknown error", null);
            }
        }
    }
}
