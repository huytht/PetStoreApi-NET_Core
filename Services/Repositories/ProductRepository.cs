using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.ProductDTO;
using PetStoreApi.Domain;
using PetStoreApi.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using PayPal.Api;
using sun.misc;
using System.Collections.Generic;

namespace PetStoreApi.Services.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        private readonly IFileRepository _fileRepository;

        private readonly IHttpContextAccessor? _httpContextAccessor;

        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(DataContext context, ILogger<ProductRepository> logger, IFileRepository fileRepository, IHttpContextAccessor? httpContextAccessor)
        {
            _context = context;
            _fileRepository = fileRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
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
                    }
                    else
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
                    foreach (IFormFile file in product.ImageFiles)
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
                var currentUsername = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                
                var appUser = currentUsername != null ? _context.AppUsers.FirstOrDefault(u => u.Username.Equals(currentUsername.Value)) : null;
                var product = _context.Products.Include("Breed").Include("Category").Include("ProductImages").Include("ProductOrigins").Include("ProductOrigins.Origin").OrderBy(product => product.Id).SingleOrDefault(product => product.Id.Equals(id));

                IEnumerable<Product> suggestionList = _context.Products.Include("ProductImages").Include("AppUserProducts").Where(p => (p.BreedId.Equals(product.BreedId) || p.CategoryId.Equals(product.CategoryId)) && !p.Id.Equals(product.Id)).Take(8);

                ProductDto result = ProductDto.CreateFromEntity(product, appUser == null ? suggestionList.Select(product => ProductShortDto.CreateFromEntity(product)) : suggestionList.Select(product => ProductShortDto.CreateFromEntity(product, appUser.Id)));

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
                var currentUsername = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                var appUser = currentUsername != null ? _context.AppUsers.FirstOrDefault(u => u.Username.Equals(currentUsername.Value)) : null;
                switch (type)
                {
                    case "all":
                        list = await _context.Products.Include("ProductImages").Include("AppUserProducts").ToListAsync();
                        break;
                    case "dog":
                        list = await _context.Products.Include("ProductImages").Include("AppUserProducts").Where(product => product.Category.Name.Contains("chó")).ToListAsync();
                        break;
                    case "cat":
                        list = await _context.Products.Include("ProductImages").Include("AppUserProducts").Where(product => product.Category.Name.Contains("mèo")).ToListAsync();
                        break;
                    case "accessory":
                        list = await _context.Products.Include("ProductImages").Include("AppUserProducts").Where(product => !product.Category.Name.Contains("mèo") && !product.Category.Name.Contains("chó")).ToListAsync();
                        break;
                    default:
                        list = await _context.Products.Include("ProductImages").Include("AppUserProducts").ToListAsync();
                        break;
                }

                IEnumerable<ProductShortDto> resultList = appUser == null ? list.Select(product => ProductShortDto.CreateFromEntity(product)) : list.Select(product => ProductShortDto.CreateFromEntity(product, appUser.Id));

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
                var currentUsername = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                var appUser = currentUsername != null ? _context.AppUsers.FirstOrDefault(u => u.Username.Equals(currentUsername.Value)) : null;
                if (filterParam.BreedId == 0)
                {
                    if (filterParam.CategoryId == 0)
                    {
                        list = await _context.Products.Include("ProductImages").Include("AppUserProducts").ToListAsync();
                    }
                    else
                    {
                        list = await _context.Products.Include("ProductImages").Include("AppUserProducts").Where(p => p.CategoryId == filterParam.CategoryId).ToListAsync();
                    }
                }
                else
                {
                    list = await _context.Products.Include("ProductImages").Include("AppUserProducts").Where(p => p.BreedId == filterParam.BreedId).ToListAsync();
                }
                IEnumerable<ProductShortDto> resultList = appUser == null ? list.Select(product => ProductShortDto.CreateFromEntity(product)) : list.Select(product => ProductShortDto.CreateFromEntity(product, appUser.Id));

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
                var currentUsername = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                var appUser = currentUsername != null ? _context.AppUsers.FirstOrDefault(u => u.Username.Equals(currentUsername.Value)) : null;
                IEnumerable<Product> list = await _context.Products.Include("ProductImages").Include("AppUserProducts").Where(p => p.Name.Contains(keyword) || p.Breed.Name.Contains(keyword) || p.Category.Name.Contains(keyword)).ToListAsync();

                IEnumerable<ProductShortDto> resultList = appUser == null ? list.Select(product => ProductShortDto.CreateFromEntity(product)) : list.Select(product => ProductShortDto.CreateFromEntity(product, appUser.Id));

                PaginatedList<ProductShortDto> resultPage = new PaginatedList<ProductShortDto>(resultList, pageParam.PageIndex, pageParam.PageSize);

                return new AppServiceResult<PaginatedList<ProductShortDto>>(true, 0, "Succeed!", resultPage);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<PaginatedList<ProductShortDto>>(false, 99, "Unknown error", null);
            }
        }

        public async Task<AppServiceResult<PaginatedList<ProductShortDto>>> GetWishList(PageParam pageParam)
        {
            try
            {
                string currentUsername = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var appUser = await _context.AppUsers.FirstOrDefaultAsync(u => u.Username.Equals(currentUsername));

                if (appUser == null)
                {
                    _logger.LogWarning("Not logged in!");

                    return new AppServiceResult<PaginatedList<ProductShortDto>>(false, 101, "Not logged in!", null);
                }

                IEnumerable<ProductShortDto> wishList = _context.AppUserProducts.Where(i => i.UserId == appUser.Id).Include("Product.ProductImages").Select(i => ProductShortDto.CreateFromEntity(i.Product));
                PaginatedList<ProductShortDto> result = new PaginatedList<ProductShortDto>(wishList, pageParam.PageIndex, pageParam.PageSize);

                return new AppServiceResult<PaginatedList<ProductShortDto>>(true, 0, "Succeed!", result);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<PaginatedList<ProductShortDto>>(false, 99, "Unknown", null);
            }
        }
        public async Task<AppBaseResult> UpdateWishList(Guid productId)
        {
            try
            {
                string currentUsername = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var appUser = await _context.AppUsers.FirstOrDefaultAsync(u => u.Username.Equals(currentUsername));

                if (appUser == null)
                {
                    _logger.LogWarning("Not logged in!");

                    return new AppServiceResult<PaginatedList<ProductShortDto>>(false, 101, "Not logged in!", null);
                }

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

                if (product == null)
                {
                    _logger.LogWarning("Product Id is not exist: " + productId + ", Cannot further process!");

                    return AppBaseResult.GenarateIsFailed(101, "Product Id is not exist: " + productId);
                }

                var existWishList = await _context.AppUserProducts.FirstOrDefaultAsync(e => e.UserId.Equals(appUser.Id) && e.ProductId.Equals(productId));

                AppUserProduct newWL = new AppUserProduct();
                newWL.ProductId = productId;
                newWL.UserId = appUser.Id;
                if (existWishList != null)
                {
                    newWL = existWishList;
                    newWL.Favourite = newWL.Favourite == null ? true : !newWL.Favourite;
                }
                else
                {
                    newWL.AppUser = appUser;
                    newWL.Product = product;
                    newWL.Favourite = true;
                    await _context.AppUserProducts.AddAsync(newWL);
                }

                await _context.SaveChangesAsync();

                return AppBaseResult.GenarateIsSucceed();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return AppBaseResult.GenarateIsFailed(99, "Unknown");
            }
        }

        public async Task<AppBaseResult> SaveRemark(RemarkProductDto remarkProduct)
        {
            try
            {
                string currentUsername = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var appUser = await _context.AppUsers.FirstOrDefaultAsync(u => u.Username.Equals(currentUsername));

                if (appUser == null)
                {
                    _logger.LogWarning("Not logged in!");

                    return new AppServiceResult<PaginatedList<ProductShortDto>>(false, 101, "Not logged in!", null);
                }

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == remarkProduct.ProductId);

                if (product == null)
                {
                    _logger.LogWarning("Product Id is not exist: " + remarkProduct.ProductId + ", Cannot further process!");

                    return AppBaseResult.GenarateIsFailed(101, "Product Id is not exist: " + remarkProduct.ProductId);
                }

                var existRemark = await _context.AppUserProducts.FirstOrDefaultAsync(e => e.UserId.Equals(appUser.Id) && e.ProductId.Equals(remarkProduct.ProductId));

                AppUserProduct newRemark = new AppUserProduct();
                newRemark.ProductId = remarkProduct.ProductId;
                newRemark.UserId = appUser.Id;
                if (existRemark != null)
                {
                    newRemark = existRemark;
                    newRemark.Rate = remarkProduct.Rate;
                    newRemark.Remark = remarkProduct.Remark;
                }
                else
                {
                    newRemark.AppUser = appUser;
                    newRemark.Product = product;
                    newRemark.Rate = remarkProduct.Rate;
                    newRemark.Remark = remarkProduct.Remark;
                    await _context.AppUserProducts.AddAsync(newRemark);
                }

                await _context.SaveChangesAsync();

                return AppBaseResult.GenarateIsSucceed();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return AppBaseResult.GenarateIsFailed(99, "Unknown");
            }
        }

        public AppServiceResult<PaginatedList<RemarkProductDto>> GetRemarkListByProduct(Guid productId, PageParam pageParam)
        {
            try
            {
                IEnumerable<RemarkProductDto> remarks = _context.AppUserProducts.Where(r => r.ProductId.Equals(productId)).Include("Product").Include("AppUser").OrderByDescending(r => r.DateModified).Select(r => RemarkProductDto.CreateFromEntity(r));

                PaginatedList<RemarkProductDto> dtoPage = new PaginatedList<RemarkProductDto>(remarks, pageParam.PageIndex, pageParam.PageSize);

                return new AppServiceResult<PaginatedList<RemarkProductDto>>(true, 0, "Succeed!", dtoPage);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return new AppServiceResult<PaginatedList<RemarkProductDto>>(false, 99, "Unknown", null);
            }
        }
    }
}
