using StudentEnrollmentApi.Models;
using StudentEnrollmentApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace StudentEnrollmentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost ("register")]
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> Register(User user)
        {
            if (await _authService.RegisterUser(user))
            {
                return Ok(new { status = "Success", message = "User Registration Successfull" });
            }

            return BadRequest(new { status = "Failed", message = "User Registration Failed" });
        }

        [HttpPost ("login")]
        public async Task<IActionResult> Login(User user)
        {
            var result = await _authService.Login(user);
            if(result == true)
            {
                var token = await _authService.GenerateToken(user);
                var role = await _authService.GetRoles(user); 
                return Ok(new {status = "Success", message = "Login Successful", data = token, roles = role
                });
            }
            return BadRequest(new {status = "fail", message = "Login Failed"});

        }


        
    }
}
