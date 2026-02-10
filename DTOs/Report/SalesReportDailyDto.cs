namespace Ecommerce_ASP.NET.DTOs.Report
{
    public class SalesReportDailyDto
    {
        public DateTime Date { get; set; }
        public int OrdersCount { get; set; }
        public decimal Revenue { get; set; }

    }
}
