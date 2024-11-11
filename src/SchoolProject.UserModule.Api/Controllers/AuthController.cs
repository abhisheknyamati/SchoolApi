using Microsoft.AspNetCore.Mvc;
using SchoolProject.UserModule.Api.DTOs;
using SchoolProject.UserModule.Api.ExceptionHandler;
using SchoolProject.UserModule.Api.Filter;
using SchoolProject.UserModule.Business.Services.Interfaces;

namespace SchoolProject.UserModule.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ModelValidationFilter))]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.Login(request.Username, request.Password);
            if (token == null)
            {
                throw new UnauthorizedAccessException(ErrorMsgConstant.InvalidToken);
            }
            return Ok(token);
        }
    }
}