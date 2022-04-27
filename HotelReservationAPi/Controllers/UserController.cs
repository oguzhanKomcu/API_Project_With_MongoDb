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
        
        
        
        [Route("authenticate")]
        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
            var token = _userRepository.Authentication(user.UserName, user.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(new { token, user });
 
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

        /// <summary>
        /// This function returns the user whose "id" is given.
        /// </summary>
        /// <param name="id">It is a required area and so type is string</param>
        /// <returns>If function is succeded will be return Ok, than will be return NotFound</returns>

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

        /// <summary>
        /// You can add a new user using this method.
        /// </summary>
        /// <returns>If function is succeded will be return CreatedAtAction, than will be return Bad Request</returns>    
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
