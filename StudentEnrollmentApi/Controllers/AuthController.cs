using StudentEnrollmentApi.Models;
using StudentEnrollmentApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

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
        public async Task<IActionResult> Register(User user)
        {
            IdentityResult result = await _authService.RegisterUser(user); 

            if (result.Succeeded)
            {
                return Ok(new { status = "Success", message = "User Registration Successfull"});
            }

            return BadRequest(new { status = "Failed", message = "User Registration Failed", data = result});
        }

        [HttpPost ("login")]
        public async Task<IActionResult> Login(User user)
        {
            var result = await _authService.Login(user);
            if(result == true)
            {
                var role = await _authService.GetRoles(user); 
                var token = _authService.GenerateToken(user);
                return Ok(new {status = "Success", message = "Login Successful", data = token, roles = role });
            }
            return BadRequest(new {status = "fail", message = "Login Failed"});
        }

        [HttpGet ("GoogleLogin")]
        [Authorize]
        public ActionResult<string> GoogleLogin()
        {
            var user = this.User;

            return "value";
        }

        
    }
}
