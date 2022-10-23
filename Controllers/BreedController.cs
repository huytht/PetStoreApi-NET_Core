using AutoMapper;
using java.security.acl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.BreedDTO;
using PetStoreApi.DTO.CategoryDTO;
using PetStoreApi.DTO.OrderDTO;
using PetStoreApi.DTO.ResponseDTO;
using PetStoreApi.Services;

namespace PetStoreApi.Controllers
{
    [Route("api/breed")]
    [ApiController]
    [Authorize(Roles = "ROLE_ADMIN")]
    public class BreedController : ControllerBase
    {
        private ILogger<BreedController> _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public BreedController(ILogger<BreedController> logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBreeds()
        {
            try
            {
                var breeds = await _repository.breedRepository.GetAllBreedsAsync();
                _logger.LogInformation($"Returned all breeds from database.");

                var breedsResult = _mapper.Map<IEnumerable<BreedDto>>(breeds);
                return Ok(new HttpResponseSuccess<IEnumerable<BreedDto>>(breedsResult));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllBreeds action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBreedById(int id)
        {
            try
            {
                var breed = await _repository.breedRepository.GetBreed(id);
                if (breed == null)
                {
                    _logger.LogError($"Breed with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned breed with id: {id}");

                    var breedResult = _mapper.Map<BreedDto>(breed);
                    return Ok(new HttpResponseSuccess<BreedDto>(breedResult)); ;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetBreedById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBreed([FromForm] BreedEditDto breed)
        {
            try
            {
                if (breed == null)
                {
                    _logger.LogError("Breed object sent from client is null.");
                    return BadRequest("Breed object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid breed object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var breedEntity = _mapper.Map<Breed>(breed);

                _repository.breedRepository.CreateBreed(breedEntity);
                await _repository.SaveAsync();

                var createdBreed = _mapper.Map<BreedDto>(breedEntity);

                return Ok(new HttpResponseSuccess<BreedDto>(createdBreed));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateBreed action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBreed(int id, [FromForm] BreedEditDto breed)
        {
            try
            {
                if (breed == null)
                {
                    _logger.LogError("Breed object sent from client is null.");
                    return BadRequest("Breed object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid breed object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var breedEntity = await _repository.breedRepository.GetBreed(id);
                if (breedEntity == null)
                {
                    _logger.LogError($"Breed with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(breed, breedEntity);

                _repository.breedRepository.UpdateBreed(breedEntity);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateBreed action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBreed(int id)
        {
            try
            {
                var breed = await _repository.breedRepository.GetBreed(id);
                if (breed == null)
                {
                    _logger.LogError($"Breed with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.breedRepository.DeleteBreed(breed);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteBreed action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
