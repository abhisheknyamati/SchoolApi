namespace SchoolProject.UserModule.Api.DTOs
{
    public class PostUserDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public bool IsAdmin { get; set; } 
    }
}