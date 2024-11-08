namespace SchoolProject.UserModule.Api.DTOs
{
    public class GetUserDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool IsAdmin { get; set; } 
    }
}