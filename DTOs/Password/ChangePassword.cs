using System.ComponentModel.DataAnnotations;

namespace Ecommerce_ASP.NET.DTOs.Password
{
    public class ChangePassword
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword
        {
            get; set;
        }
    }
}
