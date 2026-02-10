namespace Ecommerce_ASP.NET.DTOs.Report
{
    public class ProductPerformanceDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalUnitsSold { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AveragePrice { get; set; }
    }
}
