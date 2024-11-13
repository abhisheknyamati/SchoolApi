using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SchoolProject.UserModule.Business.Models;
using SchoolProject.UserModule.Business.Services.Interfaces;

namespace SchoolProject.UserModule.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> Login(User user)
        {
            var claims = await GenerateClaims(user);
            var token = GenerateToken(claims);
            return token;
        }

        private static Task<Claim[]> GenerateClaims(User user)
        {
            return Task.FromResult(new[]
            {
                new Claim("Name", user.Name ?? throw new ArgumentNullException(nameof(user.Name))),
                new Claim("Email", user.Email ?? throw new ArgumentNullException(nameof(user.Name))),
                new Claim("Role", user.IsAdmin ? "Admin" : "Teacher"),
                new Claim("Status", user.IsActive ? "Active" : "Inactive")
            });
        }

        private string GenerateToken(Claim[] claims)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key", "JWT key is not configured.");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string HashPasswordWithSalt(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16]; // 128-bit salt
                rng.GetBytes(salt); // Generate random salt

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000)) // 10,000 iterations
                {
                    byte[] hash = pbkdf2.GetBytes(32); // 256-bit hash
                    byte[] hashBytes = new byte[48]; // 16 bytes salt + 32 bytes hash

                    Array.Copy(salt, 0, hashBytes, 0, 16);
                    Array.Copy(hash, 0, hashBytes, 16, 32);

                    var hashedPwd = Convert.ToBase64String(hashBytes); // Store this result in your database
                    return hashedPwd; 
                }
            }
        }

        public bool VerifyPassword(string storedHash, string password)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(32);
                for (int i = 0; i < 32; i++)
                {
                    if (hashBytes[i + 16] != hash[i]) return false;
                }
                return true;
            }
        }
    }
}