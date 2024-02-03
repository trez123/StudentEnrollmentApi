using Microsoft.AspNetCore.Identity;
using StudentEnrollmentApi.Models;

namespace StudentEnrollmentApi.Services
{
    public interface IAuthService
    {
        Task<string?> GenerateToken(User user);
        Task<IList<string>> GetRoles(User user);
        Task<bool> Login(User user);
        Task<IdentityResult> RegisterUser(User user);
        Task<bool> AssignRoles(string userName, IEnumerable<string> roles);
    }
}