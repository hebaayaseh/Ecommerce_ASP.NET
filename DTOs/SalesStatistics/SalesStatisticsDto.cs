using Ecommerce_ASP.NET.DTOs.Order;

namespace Ecommerce_ASP.NET.DTOs.SalesStatisticsDto
{
    public class SalesStatisticsDto
    {
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public decimal totalRevenue { get; set; }
        public int totalOrders { get; set; }
        public decimal averageOrderValue { get; set; }
        
    }
}
