using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _response = new ResponseDto();

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
        }

        //StrongPass123
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequiredDto)
        {
            var errorMesage = await _authService.RegisterUserAsync(registrationRequiredDto);
            if (!string.IsNullOrEmpty(errorMesage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMesage;
                return BadRequest(_response);
            }
                
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginResponse = await _authService.LoginAsync(loginRequestDto);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid username or password";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        {
            var asignRole = await _authService.AssignRole(model.Email, model.Role.ToUpper());
            if (!asignRole)
            {
                _response.IsSuccess = false;
                _response.Message = "Failed to assign role";
                return BadRequest(_response);
            }
            return Ok(_response);
        }
    }
}
