using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.UserModule.Api.DTOs;
using SchoolProject.UserModule.Api.Filter;
using SchoolProject.UserModule.Business.Models;
using SchoolProject.UserModule.Business.Repositories.Interfaces;

namespace SchoolProject.UserModule.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ModelValidationFilter))]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public UserController(IUserRepo userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
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
            var userDto = _mapper.Map<GetUserDto>(user);
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] PostUserDto userDto)
        {
            User user = _mapper.Map<User>(userDto);
            try
            {
                User newUser = await _userRepo.AddUser(user);
                var newUserDto = _mapper.Map<GetUserDto>(newUser);
                return Ok(newUserDto);
            }
            catch (Exception ex)
            {
                throw new Exception("jai shree ram" + ex.Message);   
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _userRepo.GetUserById(id);
                await _userRepo.DeleteUser(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}