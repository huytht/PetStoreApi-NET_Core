using Microsoft.Extensions.Logging;
using PetStoreApi.Constants;
using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.ProductDTO;
using PetStoreApi.Domain;
using PetStoreApi.Helpers;
using System.Collections;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;
using Korzh.EasyQuery.Linq;

namespace PetStoreApi.Services.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        private readonly IFileRepository _fileRepository;

        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(DataContext context, ILogger<ProductRepository> logger, IFileRepository fileRepository)
        {
            _context = context;
            _fileRepository = fileRepository;
            _logger = logger;
        }

        public AppServiceResult<Product> AddProduct(ProductCreateDto product)
        {
            try
            {
                Product newProduct = new Product();
                newProduct.Id = Guid.NewGuid();
                newProduct.Name = product.Name;
                if (product.BreedId != null)
                {
                    var breed = _context.Breeds.FirstOrDefault(b => b.Id == product.BreedId);

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
                    var category = _context.Categories.FirstOrDefault(c => c.Id == product.CategoryId);

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
                        var origin = _context.Origins.FirstOrDefault(o => o.Id == originId);

                        if (origin != null)
                        {
                            ProductOrigin productOrigin = new ProductOrigin();
                            productOrigin.ProductId = newProduct.Id;
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
                _context.Add(newProduct);
                _context.SaveChanges();

                //ProductDto dto = ProductDto.CreateFromEntity(newProduct);

                return new AppServiceResult<Product>(true, 0, "Succeed!", newProduct);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<Product>(false, 99, "Unknown error", null);
            }
        }

        public AppServiceResult<ProductDto> GetProductById(Guid id)
        {
            try
            {
                var product = _context.Products.Include("Breed").Include("Category").Include("ProductImages").Include("ProductOrigins").Include("ProductOrigins.Origin").OrderBy(product => product.Id).SingleOrDefault(product => product.Id.Equals(id));

                IEnumerable<Product> suggestionList = _context.Products.Include("ProductImages").Where(p => (p.BreedId.Equals(product.BreedId) || p.CategoryId.Equals(product.CategoryId)) && !p.Id.Equals(product.Id)).Take(8);
                
                ProductDto result = ProductDto.CreateFromEntity(product, suggestionList.Select(p => ProductShortDto.CreateFromEntity(p)));

                return new AppServiceResult<ProductDto>(true, 0, "Succeed!", result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<ProductDto>(false, 99, "Unknown error", null);
            }
        }

        public async Task<AppServiceResult<PaginatedList<ProductShortDto>>> GetProductList(PageParam pageParam, string type = "all")
        {
            try
            {
                IEnumerable<Product> list;
                switch (type)
                {
                    case "all": list = await _context.Products.Include("ProductImages").ToListAsync();
                        break;
                    case "dog": list = await _context.Products.Include("ProductImages").Where(product => product.Category.Name.Contains("chó")).ToListAsync();
                        break;
                    case "cat": list = await _context.Products.Include("ProductImages").Where(product => product.Category.Name.Contains("mèo")).ToListAsync();
                                break;
                    case "accessory": list = await _context.Products.Include("ProductImages").Where(product => !product.Category.Name.Contains("mèo") && !product.Category.Name.Contains("chó")).ToListAsync();
                                break;
                    default: list = await _context.Products.Include("ProductImages").ToListAsync();
                             break;
                }
                
                IEnumerable<ProductShortDto> resultList = list.Select(product => ProductShortDto.CreateFromEntity(product));

                PaginatedList<ProductShortDto> resultPage = new PaginatedList<ProductShortDto>(resultList, pageParam.PageIndex, pageParam.PageSize);

                return new AppServiceResult<PaginatedList<ProductShortDto>>(true, 0, "Succeed!", resultPage);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<PaginatedList<ProductShortDto>>(false, 99, "Unknown error", null);
            }
        }

        public async Task<AppServiceResult<PaginatedList<ProductShortDto>>> GetProductFilterList(PageParam pageParam, FilterParam filterParam)
        {
            try
            {
                IEnumerable<Product> list;
                if (filterParam.BreedId == 0)
                {
                    if (filterParam.CategoryId == 0)
                    {
                        list = await _context.Products.Include("ProductImages").ToListAsync();
                    } else
                    {
                        list = await _context.Products.Include("ProductImages").Where(p => p.CategoryId == filterParam.CategoryId).ToListAsync();
                    }
                } else
                {
                    list = await _context.Products.Include("ProductImages").Where(p => p.BreedId == filterParam.BreedId).ToListAsync();
                }
                IEnumerable<ProductShortDto> resultList = list.Select(product => ProductShortDto.CreateFromEntity(product));

                PaginatedList<ProductShortDto> resultPage = new PaginatedList<ProductShortDto>(resultList, pageParam.PageIndex, pageParam.PageSize);

                return new AppServiceResult<PaginatedList<ProductShortDto>>(true, 0, "Succeed!", resultPage);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<PaginatedList<ProductShortDto>>(false, 99, "Unknown error", null);
            }
        }

        public async Task<AppServiceResult<PaginatedList<ProductShortDto>>> SearchProduct(PageParam pageParam, string keyword)
        {
            try
            {
                IEnumerable<Product> list = await _context.Products.Include("ProductImages").Where(p => p.Name.Contains(keyword) || p.Breed.Name.Contains(keyword) || p.Category.Name.Contains(keyword)).ToListAsync();

                IEnumerable<ProductShortDto> resultList = list.Select(product => ProductShortDto.CreateFromEntity(product));
                
                PaginatedList<ProductShortDto> resultPage = new PaginatedList<ProductShortDto>(resultList, pageParam.PageIndex, pageParam.PageSize);

                return new AppServiceResult<PaginatedList<ProductShortDto>>(true, 0, "Succeed!", resultPage);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<PaginatedList<ProductShortDto>>(false, 99, "Unknown error", null);
            }
        }
    }
}
