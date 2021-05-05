using Blogging.Models;
using Blogging.Services.UsersDBService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Blogging.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public UsersController(IUsersService usersService)
        {
            _service = usersService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDetails userDetails)
        {
            var token = await _service.Login(userDetails.Email, userDetails.Password);
            if(token == null)
            {
                return Unauthorized();
            }

            return Ok(new { token, userDetails });
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> CreateUser([FromBody] UserDetails details)
        {
            var response = await  _service.CreateUser(details.Email, details.Password);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return Ok(content);
            }

            return BadRequest(content);
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser([FromQuery] string userId)
        {
            var response = await _service.GetUser(userId);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<User>(content);
                return Ok(result);
            }

            return BadRequest(content);
        }

        private readonly IUsersService _service;
    }
}
