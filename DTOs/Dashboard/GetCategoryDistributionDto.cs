namespace Ecommerce_ASP.NET.DTOs.Dashboard
{
    public class GetCategoryDistributionDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int productCount { get; set; }
    }
}
