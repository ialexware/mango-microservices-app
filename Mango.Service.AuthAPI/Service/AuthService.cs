using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerato _jwtTokenGenerato;


        public AuthService(AppDbContext db, IJwtTokenGenerato jwtTokenGenerato, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _jwtTokenGenerato = jwtTokenGenerato;
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName.ToLower() == email.ToLower());
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    // Create the role if it does not exist
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }

                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;

        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (user == null || !isPasswordValid)
            {
                return new LoginResponseDto()
                {
                    User = null,
                    Token = String.Empty
                };
            }
            // If user was found, generate JWT token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerato.CreateToken(user, roles);

            UserDto userDto = new()
            {
                ID = user.Id,
                Email = user.Email,
                Name = user.FirstName,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = token,
            };

            return loginResponseDto;
        }

        public async Task<string> RegisterUserAsync(RegistrationRequestDto registrationRequiredDto)
        {
            try
            {
                ApplicationUser user = new()
                {
                    UserName = registrationRequiredDto.Email,
                    Email = registrationRequiredDto.Email,
                    NormalizedEmail = registrationRequiredDto.Email.ToUpper(),
                    FirstName = registrationRequiredDto.FirstName,
                    PhoneNumber = registrationRequiredDto.PhoneNumber,
                };

                var result = await _userManager.CreateAsync(user, registrationRequiredDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = await _db.Users.Where(x => x.Email == registrationRequiredDto.Email).FirstOrDefaultAsync();
                    UserDto userDto = new()
                    {
                        ID = userToReturn.Id,
                        Email = userToReturn.Email,
                        Name = userToReturn.FirstName,
                        PhoneNumber = userToReturn.PhoneNumber
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {


                throw;
            }

            return "Error Encountered";


        }
    }
}
