using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using StudentEnrollmentApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StudentEnrollmentApi.Services;

namespace WukkamanCleaningAgencyApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        public AuthService(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            this._userManager = userManager;
            this._config = config;
        }
        public async Task<bool> RegisterUser(User user)
        {
            IdentityUser identityUser = new()
            {
                UserName = user.UserName,
                Email = user.UserName
            };

            IdentityResult result = await _userManager.CreateAsync(identityUser, user.Password);

            return result.Succeeded;
        }

        public async Task<bool> Login(User user)
        {
            IdentityUser? identityUser = await _userManager.FindByEmailAsync(user.UserName);

            if (identityUser == null)
            {
                return false;
            }
            else
            {
                if(await _userManager.CheckPasswordAsync(identityUser,user.Password))
                {
                    return true;
                }
                return false;
            }
        }

        public async Task<bool> AssignRoles(string userName, IEnumerable<string> roles)
        {
            IdentityUser? user = await _userManager.FindByNameAsync(userName);
            if(user == null)
            {
                return false;
            }
            IdentityResult? result = await _userManager.AddToRolesAsync(user, roles);
            return result.Succeeded;
        }

        public async Task<string?> GenerateToken(User user)
        {
            // IEnumerable<Claim> claims = new List<Claim>
            // {
            //     new(ClaimTypes.Email, user.UserName),
            //     new(ClaimTypes.Role, "Admin"),
            // };

            IdentityUser? identityUser = await _userManager.FindByEmailAsync(user.UserName);
            if(identityUser == null)
            {
                return null;
            }

            IList<string> userRoles = await _userManager.GetRolesAsync(identityUser);

            List<Claim> claims = new()
            {
                new(ClaimTypes.Email, user.UserName),  
            };

            if(userRoles.Any())
            {
                claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_config.GetSection("JWTConfig:Key").Value!));

            SigningCredentials signingCreds = new(securityKey, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken securityToken = new(
                claims : claims, 
                expires: DateTime.Now.AddDays(1),
                issuer: _config.GetSection("JWTConfig:Issuer").Value,
                audience: _config.GetSection("JWTConfig:Audience").Value,
                signingCredentials: signingCreds
            );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }

        public async Task<IList<string>> GetRoles(User user)
        {
            IdentityUser identityUser = new()
            {
                UserName = user.UserName,
                Email = user.UserName
            };

            return await _userManager.GetRolesAsync(identityUser);
        }
    }
}
