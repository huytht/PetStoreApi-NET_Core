using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetStoreApi.Data.Entity;
using PetStoreApi.Domain;
using PetStoreApi.DTO.BreedDTO;
using PetStoreApi.DTO.CategoryDTO;
using PetStoreApi.DTO.OrderStatusDTO;
using PetStoreApi.DTO.OriginDTO;
using PetStoreApi.Helpers;

namespace PetStoreApi.Services.Repositories
{
    public class CommonRepository : ICommonRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<CommonRepository> _logger;

        public CommonRepository(DataContext context, ILogger<CommonRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<AppServiceResult<List<Breed?>>> GetAllBreedByCategory(int categoryId)
        {
            try
            {
                List<Breed?> result;

                if (categoryId != 0)
                {
                    var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

                    if (category == null)
                    {
                        _logger.LogWarning("Category Id: " + categoryId + " is not exist!");

                        return new AppServiceResult<List<Breed?>>(false, 101, "Category Id: " + categoryId + " is not exist!", null);
                    }
                    result = await _context.Products.Where(p => p.CategoryId == categoryId).Select(p => p.Breed).Distinct().ToListAsync();
                }
                else
                {
                    result = await _context.Breeds.Select(b => b).ToListAsync();
                }
                return new AppServiceResult<List<Breed?>>(true, 0, "Succeed!", result);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<List<Breed?>>(false, 99, "Unknown", null);
            }
        }

        public async Task<AppServiceResult<List<BreedDto>>> GetBreedList()
        {
            try
            {
                List<BreedDto> result = await _context.Breeds.Select(b => BreedDto.CreateFromEntity(b)).ToListAsync();

                return new AppServiceResult<List<BreedDto>>(true, 0, "Succeed!", result);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<List<BreedDto>>(false, 99, "Unknown", null);
            }
        }

        public async Task<AppServiceResult<List<CategoryDto>>> GetCategoryList()
        {
            try
            {
                List<CategoryDto> result = await _context.Categories.Select(c => CategoryDto.CreateFromEntity(c)).ToListAsync();

                return new AppServiceResult<List<CategoryDto>>(true, 0, "Succeed!", result);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<List<CategoryDto>>(false, 99, "Unknown", null);
            }
        }

        public async Task<AppServiceResult<List<OrderStatusDto>>> GetOrderStatusList()
        {
            try
            {
                List<OrderStatusDto> result = await _context.OrderStatuses.Select(o => OrderStatusDto.CreateFromEntity(o)).ToListAsync();

                return new AppServiceResult<List<OrderStatusDto>>(true, 0, "Succeed!", result);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<List<OrderStatusDto>>(false, 99, "Unknown", null);
            }
        }

        public async Task<AppServiceResult<List<OriginDto>>> GetOriginList()
        {
            try
            {
                List<OriginDto> result = await _context.Origins.Select(o => OriginDto.CreateFromEntity(o)).ToListAsync();

                return new AppServiceResult<List<OriginDto>>(true, 0, "Succeed!", result);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<List<OriginDto>>(false, 99, "Unknown", null);
            }
        }
    }
}
