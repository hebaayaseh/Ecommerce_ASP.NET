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
        public int price { get; set; }
        [Required]
        public int quantity { get; set; }
        [Required]
        public string image { get; set; }
        public Categories category { get; set; }

    }
}
