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

        public IActionResult Get(Guid id)
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

        public IActionResult Post(UserModelDto user)
        {
            var model = _userMapper.ToModel(user);
            _userRepository.Create(model);

            return Accepted();
        }
        
        public IActionResult Put(UserModelDto user)
        {
            var model = _userMapper.ToModel(user);
            _userRepository.Update(model);
            
            return Accepted();
        }

        public IActionResult Delete(Guid id)
        {
            _userRepository.Remove(id);
            
            return Accepted();
        }

    }
}