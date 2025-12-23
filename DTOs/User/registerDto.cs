using Ecommerce_ASP.NET.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_ASP.NET.DTOs.User
{
    public class registerDto
    {
        [Required]
        public string f_Name { get; set; }
        

        public string? l_Name { get; set; }
        [Required]

        public string Email { get; set; }
        [Required]

        public string Password { get; set; }
        [Required]
        public string confirmPassword { get; set; }

        [Required]
        public string Phone { get; set; }
        [Required]
        public UserRole role { get; set; }
    }
}
