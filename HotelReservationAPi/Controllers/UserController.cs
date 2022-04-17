using HotelReservationAPi.Infrastructure.Repositories.Interface;
using HotelReservationAPi.Model.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace HotelReservationAPi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        
        [AllowAnonymous]
        [Route("authenticate")]
        [HttpPost]
        public IActionResult Login([FromBody] User model)
        {
            var user = _userRepository.Authentication(model.UserName, model.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(new { user, model});
 
        }

        /// <summary>
        /// This function lists all made user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userRepository.GetUsers().ConfigureAwait(false));
        }

        [HttpGet("{id:length(24)}")]
        public  ActionResult<User> GetUser(string id)
        {
            var user =  _userRepository.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        
        [HttpPost("Register") ]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (user is null)
            {
                return BadRequest();
            }

            await _userRepository.Register(user);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }




    }
}
