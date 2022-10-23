using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.CategoryDTO;
using PetStoreApi.DTO.ResponseDTO;
using PetStoreApi.Services;

namespace PetStoreApi.Controllers
{
    [Route("api/category")]
    [ApiController]
    [Authorize(Roles = "ROLE_ADMIN")]
    public class CategoryController : ControllerBase
    {
        private ILogger<CategoryController> _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public CategoryController(ILogger<CategoryController> logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategorys()
        {
            try
            {
                var categories = await _repository.categoryRepository.GetAllCategoriesAsync();
                _logger.LogInformation($"Returned all categorys from database.");

                var categorysResult = _mapper.Map<IEnumerable<CategoryDto>>(categories);
                return Ok(new HttpResponseSuccess<IEnumerable<CategoryDto>>(categorysResult));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllCategorys action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _repository.categoryRepository.GetCategory(id);
                if (category == null)
                {
                    _logger.LogError($"Category with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned category with id: {id}");

                    var categoryResult = _mapper.Map<CategoryDto>(category);
                    return Ok(new HttpResponseSuccess<CategoryDto>(categoryResult));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetCategoryById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryEditDto category)
        {
            try
            {
                if (category == null)
                {
                    _logger.LogError("Category object sent from client is null.");
                    return BadRequest("Category object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid category object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var categoryEntity = _mapper.Map<Category>(category);

                _repository.categoryRepository.CreateCategory(categoryEntity);
                await _repository.SaveAsync();

                var createdCategory = _mapper.Map<CategoryDto>(categoryEntity);

                return Ok(new HttpResponseSuccess<CategoryDto>(createdCategory));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateCategory action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] CategoryEditDto category)
        {
            try
            {
                if (category == null)
                {
                    _logger.LogError("Category object sent from client is null.");
                    return BadRequest("Category object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid category object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var categoryEntity = await _repository.categoryRepository.GetCategory(id);
                if (categoryEntity == null)
                {
                    _logger.LogError($"Category with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(category, categoryEntity);

                _repository.categoryRepository.UpdateCategory(categoryEntity);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateCategory action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _repository.categoryRepository.GetCategory(id);
                if (category == null)
                {
                    _logger.LogError($"Category with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.categoryRepository.DeleteCategory(category);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteCategory action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
