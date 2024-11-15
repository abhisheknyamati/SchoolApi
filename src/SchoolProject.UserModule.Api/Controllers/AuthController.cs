using Microsoft.AspNetCore.Mvc;
using SchoolProject.UserModule.Api.DTOs;
using SchoolProject.UserModule.Api.ExceptionHandler;
using SchoolProject.UserModule.Api.Filter;
using SchoolProject.UserModule.Business.Repositories.Interfaces;
using SchoolProject.UserModule.Business.Services.Interfaces;

namespace SchoolProject.UserModule.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAdminRepo _adminRepo;

        public AuthController(IAuthService authService, IAdminRepo adminRepo)
        {
            _authService = authService;
            _adminRepo = adminRepo;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _adminRepo.GetUserByEmail(request.Email);
            if (user != null && user.Password != null)
            {
                if (user.IsActive)
                {
                    var verificationStatus = _authService.VerifyPassword(user.Password, request.Password);
                    if (verificationStatus)
                    {
                        var token = await _authService.Login(user);
                        if (token != null)
                        {
                            return Ok(token);
                        }
                        throw new UnauthorizedAccessException(ErrorMsgConstant.InvalidToken);
                    }
                    return Unauthorized(ErrorMsgConstant.InvalidPassword);
                }
                return Unauthorized(ErrorMsgConstant.UserNotActive);
            }
            return NotFound(ErrorMsgConstant.UserNotFound);
        }
    }
}