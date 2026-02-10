namespace Ecommerce_ASP.NET.DTOs.Report
{
    public class SalesReportDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }

        public List<SalesReportDailyDto> DailySales { get; set; }

    }
}
