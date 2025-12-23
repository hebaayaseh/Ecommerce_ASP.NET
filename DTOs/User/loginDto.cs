using Ecommerce_ASP.NET.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_ASP.NET.DTOs.User
{
    public class loginDto
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        
    }
}
