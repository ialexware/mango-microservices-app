using Mango.Services.AuthAPI.Models;

namespace Mango.Services.AuthAPI.Service.IService
{
    public interface IJwtTokenGenerato
    {
        string CreateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
    }
}
