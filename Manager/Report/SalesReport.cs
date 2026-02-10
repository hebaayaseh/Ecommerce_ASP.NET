using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Report;
using Ecommerce_ASP.NET.Models.Enums;
namespace Ecommerce_ASP.NET.Manager.Report
{
    public class ReportManager
    {
        public readonly AppDbContext dbContext;
        public ReportManager(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public SalesReportDto SalesReport(DateTime from, DateTime To)
        {
            var fromDate = from.Date;
            var toDate = To.Date.AddDays(1).AddTicks(-1);
            var order = dbContext.Orders.Where(o => o.status == OrderStatus.Delivered &&
            o.created_at <= fromDate && o.created_at >= toDate).ToList();
            int totalOrders = order.Count();
            var payment = dbContext.payments.Where(p => p.PaymentDate <= fromDate && p.PaymentDate >= toDate
            && p.Status == PaymentStatus.Completed).ToList();
            var totalRevenue = payment.Sum(p => p.Amount);
            var averageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0;
            var dailySales = order
            .GroupBy(o => o.created_at.Date)
            .Select(g => new SalesReportDailyDto
            {
                Date = g.Key,
                OrdersCount = g.Count(),
                Revenue = dbContext.payments
                .Where(p =>
                    p.Status == PaymentStatus.Completed &&
                    p.PaymentDate.Date == g.Key
                )
                .Sum(p => p.Amount)
            })
            .OrderBy(d => d.Date)
            .ToList();
            return new SalesReportDto
            {
                From = fromDate,
                To = toDate,
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                AverageOrderValue = averageOrderValue,
                DailySales = dailySales
            };
        }
    }
}
