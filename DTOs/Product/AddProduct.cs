using Ecommerce_ASP.NET.DTOs.Category;
using Ecommerce_ASP.NET.Models;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_ASP.NET.DTOs.Product
{
    public class AddProduct
    {
        [Required]
        public string name {  get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public decimal price { get; set; }
        [Required]
        public int stock { get; set; }
        [Required]
        public string image { get; set; }
        public CategoryDto category { get; set; }

    }
}
