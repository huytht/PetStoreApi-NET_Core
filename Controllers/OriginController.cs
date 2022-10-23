using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetStoreApi.Data.Entity;
using PetStoreApi.DTO.CategoryDTO;
using PetStoreApi.DTO.OriginDTO;
using PetStoreApi.DTO.ResponseDTO;
using PetStoreApi.Services;

namespace PetStoreApi.Controllers
{
    [Route("api/origin")]
    [ApiController]
    [Authorize(Roles = "ROLE_ADMIN")]
    public class OriginController : ControllerBase
    {
        private ILogger<OriginController> _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public OriginController(ILogger<OriginController> logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrigins()
        {
            try
            {
                var origins = await _repository.originRepository.GetAllOriginsAsync();
                _logger.LogInformation($"Returned all origins from database.");

                var originsResult = _mapper.Map<IEnumerable<OriginDto>>(origins);
                return Ok(new HttpResponseSuccess<IEnumerable<OriginDto>>(originsResult)); ;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllOrigins action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOriginById(int id)
        {
            try
            {
                var origin = await _repository.originRepository.GetOrigin(id);
                if (origin == null)
                {
                    _logger.LogError($"Origin with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned origin with id: {id}");

                    var originResult = _mapper.Map<OriginDto>(origin);
                    return Ok(new HttpResponseSuccess<OriginDto>(originResult));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOriginById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrigin([FromForm] OriginEditDto origin)
        {
            try
            {
                if (origin == null)
                {
                    _logger.LogError("Origin object sent from client is null.");
                    return BadRequest("Origin object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid origin object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var originEntity = _mapper.Map<Origin>(origin);

                _repository.originRepository.CreateOrigin(originEntity);
                await _repository.SaveAsync();

                var createdOrigin = _mapper.Map<OriginDto>(originEntity);

                return Ok(new HttpResponseSuccess<OriginDto>(createdOrigin));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateOrigin action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrigin(int id, [FromForm] OriginEditDto origin)
        {
            try
            {
                if (origin == null)
                {
                    _logger.LogError("Origin object sent from client is null.");
                    return BadRequest("Origin object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid origin object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var originEntity = await _repository.originRepository.GetOrigin(id);
                if (originEntity == null)
                {
                    _logger.LogError($"Origin with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(origin, originEntity);

                _repository.originRepository.UpdateOrigin(originEntity);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateOrigin action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrigin(int id)
        {
            try
            {
                var origin = await _repository.originRepository.GetOrigin(id);
                if (origin == null)
                {
                    _logger.LogError($"Origin with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.originRepository.DeleteOrigin(origin);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteOrigin action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
