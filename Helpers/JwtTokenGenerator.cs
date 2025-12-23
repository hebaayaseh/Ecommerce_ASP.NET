using Duende.IdentityServer.Models;
using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce_ASP.NET.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;

        public JwtHelper(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public string? Authenticate(loginDto userdto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.email == userdto.email);
            if (user == null)
                return null;

            var hasher = new PasswordHasher();
            if (!hasher.Verify(userdto.password, user.passwordHash))
                return null;

            var claims = new List<Claim>
            {
        new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
        new Claim(ClaimTypes.Email, user.email),
        new Claim(ClaimTypes.Role, user.role.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(60),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
