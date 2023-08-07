using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new LoginRequestDto();
            return View(loginRequestDto);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            ResponseDto response = await _authService.LoginAsync(loginRequest);
            if (response != null && response.IsSuccess)
            {
                LoginResponseDto loginResponseDto =
                    JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));

                await SignInUser(loginResponseDto);
                _tokenProvider.SetToken(loginResponseDto.Token);

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                TempData["error"] = response.Message;
                return View(loginRequest);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem(){Text = SD.RoleAdmin, Value = SD.RoleAdmin},
                new SelectListItem(){Text = SD.RoleCustomer, Value = SD.RoleCustomer},
            };
            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegistrationRequiredDto registration)
        {
            ResponseDto response = await _authService.RegisterAsync(registration);
            if (response != null && response.IsSuccess)
            {
                if (string.IsNullOrEmpty(registration.Role))
                {
                    registration.Role = SD.RoleCustomer;
                }
                ResponseDto responseRole = await _authService.AssignRoleAsync(registration);
                if (responseRole != null && responseRole.IsSuccess)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                } 

            }
            else
            {
                TempData["error"] = response.Message;
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem(){Text = SD.RoleAdmin, Value = SD.RoleAdmin},
                new SelectListItem(){Text = SD.RoleCustomer, Value = SD.RoleCustomer},
            };
            ViewBag.RoleList = roleList;
            return View(registration);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.RemoveToken();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private async Task SignInUser(LoginResponseDto loginResponseDto)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(loginResponseDto.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(
                 new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value)
                );
            identity.AddClaim(
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value)
                );
            identity.AddClaim(
                new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value)
                );

            identity.AddClaim(
               new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value)
               );

            identity.AddClaim(
                new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value)
                );

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
