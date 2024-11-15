using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.UserModule.Api.DTOs;
using SchoolProject.UserModule.Api.ExceptionHandler;
using SchoolProject.UserModule.Api.Filter;
using SchoolProject.UserModule.Business.Models;
using SchoolProject.UserModule.Business.Repositories.Interfaces;
using SchoolProject.UserModule.Business.Services.Interfaces;

namespace SchoolProject.UserModule.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UserController(IUserRepo userRepo, IMapper mapper, IAuthService authService)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepo.GetAllUsers();
            var userDtos = _mapper.Map<IList<GetUserDto>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepo.GetUserById(id);
            if (user != null)
            {
                var userDto = _mapper.Map<GetUserDto>(user);
                return Ok(userDto);
            }
            return NotFound(ErrorMsgConstant.UserNotFound);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(PostUserDto userDto)
        {
            userDto.Password = _authService.HashPasswordWithSalt(userDto.Password);
            User user = _mapper.Map<User>(userDto);
            User newUser = await _userRepo.AddUser(user);
            if (newUser != null)
            {
                var newUserDto = _mapper.Map<GetUserDto>(newUser);
                return Ok(newUserDto);
            }
            return BadRequest(ErrorMsgConstant.UserNotCreated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepo.GetUserById(id);
            if (user != null)
            {
                bool success = await _userRepo.DeleteUser(user);
                if (success)
                {
                    var userDto = _mapper.Map<GetUserDto>(user);
                    return Ok(userDto);
                }
                throw new Exception(ErrorMsgConstant.UserAlreadyDeleted);
            }
            return NotFound(ErrorMsgConstant.UserNotFound);
        }
    }
}