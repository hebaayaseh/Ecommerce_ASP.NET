using Ecommerce_ASP.NET.Manager;

namespace Ecommerce_ASP.NET.DTOs.Review
{
    public class ReviewDto
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int productId { get; set; }
        public int rating { get; set; }
        public string comment { get; set; }
        public DateTime DateTime { get; set; }
        public ReviewDto()
        {
            DateTime = DateTime.Now;
        }
    }
}
