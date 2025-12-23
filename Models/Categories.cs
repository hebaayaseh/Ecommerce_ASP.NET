using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_ASP.NET.Models
{
    public class Categories
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public ICollection<Products> Products { get; set; }
        
        public Categories()
        {
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }
    }
}
