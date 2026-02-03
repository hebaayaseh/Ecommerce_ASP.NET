using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.User;
using Ecommerce_ASP.NET.Helpers;
using Ecommerce_ASP.NET.Models;

namespace Ecommerce_ASP.NET.Manager
{
    public class AuthManager
    {
        public readonly AppDbContext _context;
        private readonly PasswordHasher passwordHasher;
        public AuthManager(AppDbContext context, PasswordHasher passwordHasher)
        {
            _context = context;
            this.passwordHasher = passwordHasher;
        }
        public void RegisterDto(registerDto userDto , string passwordHash)
        {
            var user = new User
            {
                f_name = userDto.f_Name,
                l_name = userDto.l_Name,
                email = userDto.Email,
                passwordHash = passwordHash,
                role = userDto.role,
                phone = userDto.Phone
            };
            _context.Users.Add(user);
             _context.SaveChanges();
            
        }
        public void GetUserByEmail(string email,string newPassword)
        {
            var user = _context.Users.FirstOrDefault(u => u.email == email);
            if (user == null) throw new KeyNotFoundException("User not found");
            if (string.IsNullOrEmpty(newPassword))
                throw new ArgumentException("New password cannot be empty"); 
            newPassword = newPassword.Trim();

            user.passwordHash = passwordHasher.Hash(newPassword);
            _context.SaveChanges();
        }
        public void ResetPassword(string email, string newPassword)
        {
            var user = _context.Users.FirstOrDefault(u => u.email == email);
            if (user == null) throw new KeyNotFoundException("User not found");
            user.passwordHash = passwordHasher.Hash(newPassword);
            _context.SaveChanges();
        }
    }
}
