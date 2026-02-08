namespace Ecommerce_ASP.NET.DTOs.Dashboard
{
    public class GetTopProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalQuantitySold { get; set; }
    }
}
