using Microsoft.AspNetCore.Mvc;
using Work.ApiModels;
using Work.Database;
using Work.Interfaces;
using Work.Mappers;

namespace Work.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IMapper<User, UserModelDto> _userMapper;
        private readonly ILogger<UserController> _logger;

        // Using interfaces here may not be necessary if there is only single implementation of each interface.
        // But I leave it as is because afaik DI using interfaces is a standard practice in .NET 
        // Async is not used for methods because there are no actual async operations behind DB operations
        public UserController(
            IRepository<User, Guid> userRepository, 
            IMapper<User, UserModelDto> userMapper,
            ILogger<UserController> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
            _logger = logger;
        }

        /// <summary>
        /// Create user by the given dto
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Ok if user was created, Not Found if user does not exist</returns>
        [HttpGet]
        [Route("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserModelDto> Get(Guid id)
        {
            try
            {
                var user = _userRepository.Read(id);

                return Ok(_userMapper.ToDto(user));
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError(e, $"{nameof(User)} was not found by Id {id}");
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Create user by the given dto
        /// </summary>
        /// <returns>Accepted if user was created</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] UserModelDto user)
        {
            var model = _userMapper.ToModel(user);
            _userRepository.Create(model);

            _logger.LogInformation("User {@User} was created.", user);

            return Accepted();
        }
        
        /// <summary>
        /// Update user with the given Id (in dto)
        /// </summary>
        /// <returns>Accepted if user was updated, Not Found if user does not exist</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Put([FromBody] UserModelDto user)
        {
            try
            {
                var model = _userMapper.ToModel(user);
                _userRepository.Update(model);
                
                return Accepted();
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError(e, $"{nameof(User)} was not found by Id {user.Id}");
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Delete user by the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Accepted if user was removed, Not Found if user does not exist</returns>
        [HttpDelete]
        [Route("id")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _userRepository.Remove(id);
            
                return Accepted();
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError(e, $"{nameof(User)} was not found by Id {id}");
                return NotFound(e.Message);
            }
        }
    }
}