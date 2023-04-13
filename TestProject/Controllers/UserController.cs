using Microsoft.AspNetCore.Mvc;
using Work.ApiModels;
using Work.Database;
using Work.Interfaces;
using Work.Mappers;

namespace Work.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IMapper<User, UserModelDto> _userMapper;
        
        // Using interfaces here may not be necessary if there is only single implementation of each interface
        // I prefer to avoid using DI for the sake of DI, but leave it for demonstration purposes (and unit tests)
        public UserController(IRepository<User, Guid> userRepository, IMapper<User, UserModelDto> userMapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
        }

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
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        [Route("id")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] UserModelDto user)
        {
            var model = _userMapper.ToModel(user);
            _userRepository.Create(model);

            return Accepted();
        }
        
        [HttpPut]
        [Route("id")]
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
                return NotFound(e.Message);
            }
        }

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
                return NotFound(e.Message);
            }
        }
    }
}