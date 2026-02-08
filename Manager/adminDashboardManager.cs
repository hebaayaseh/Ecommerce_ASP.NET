using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Dashboard;
using Ecommerce_ASP.NET.DTOs.Order;
using Ecommerce_ASP.NET.DTOs.SalesStatisticsDto;
using Ecommerce_ASP.NET.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace Ecommerce_ASP.NET.Manager
{
    public class adminDashboardManager
    {
        public readonly AdminDashboardDto adminDashboardDto;
        public readonly AppDbContext dbContext;
        public adminDashboardManager(AppDbContext dbContext, AdminDashboardDto adminDashboardDto)
        {
            this.dbContext = dbContext;
            this.adminDashboardDto = adminDashboardDto;
        }
        public AdminDashboardDto GetOverView()
        {
            int totalUsers = dbContext.Users.Count();
            var totalProducts=dbContext.Products.Count();
            int totalOrders=dbContext.Orders.Where(o=>o.status==OrderStatus.Delivered).Count();
            decimal totalRevenue = dbContext.payments.Where(p=>p.Status==PaymentStatus.Completed).Sum(p => p.Amount);
            decimal averageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0;
            
            return new AdminDashboardDto
            {
                TotalUsers = totalUsers,
                TotalProducts = totalProducts,
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                AverageOrderValue = averageOrderValue,
                
            };
        }
        public List<OrderByStatusDto> OrderByStatus()
        {
            var orderStatus = dbContext.Orders.GroupBy(s => s.status)
                .Select(s => new OrderByStatusDto
                {
                    status = s.Key.ToString(),
                    count = s.Count()
                }).ToList();

            return orderStatus;
        }
        public List<ProductsByCategoryDto> productsByCategories()
        {
            var productsByCategory = dbContext.Products
                .GroupBy(p => new { p.categoryId, p.category.name})
                .Select(p=>new ProductsByCategoryDto
                {
                    categoryName=p.Key.ToString(),
                    productcount=p.Count()
                }).ToList();
            return productsByCategory;
        }
        public SalesStatisticsDto SalesStatistics(DateTime from , DateTime to)
        {
            var fromDate = from.Date;
            var toDate = to.Date.AddDays(1).AddTicks(-1);
            var totalRevenue = dbContext.payments.Where(d=>d.PaymentDate>=fromDate&&d.PaymentDate<=toDate&&d.Status==PaymentStatus.Completed).Sum(p => p.Amount);
            var totalOrders =dbContext.Orders.Where(o=>o.created_at>=fromDate&&o.created_at<=toDate&&o.status==OrderStatus.Delivered).Count();
            var averageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0;
            return new SalesStatisticsDto
            {
                from = fromDate,
                to = toDate,
                totalRevenue = totalRevenue,
                totalOrders = totalOrders,
                averageOrderValue = averageOrderValue,
                
            };
        }
        public List<GetTopSellingProductsDto>? GetTopSellingProducts(int limit = 10)
        {
            var TopSellingProducts = dbContext.OrderItems
                .Where(o=>o.Order.status==OrderStatus.Delivered)
                .GroupBy(p=>p.ProductId)
                .Select(p=> new GetTopSellingProductsDto{
                 Quantity=p.Sum(q=>q.quantity),
                 ProductId=p.Key,
                });
            var topProducts = TopSellingProducts.OrderByDescending(p => p.Quantity).Take(limit).ToList();
            return topProducts;
        }
        public List<GetTopCustomersDto> GetTopCustomers(int limit = 10)
        {
            var topCustomers = dbContext.payments
                .Where(p => p.Status == PaymentStatus.Completed)
                .GroupBy(u=>u.UserId)
                .Select(u=> new GetTopCustomersDto
                {
                    CustomerId=u.Key.id,
                    CustomerName=u.Key.f_name,
                    TotalSpent=u.Sum(o=>o.orders.Where(s=>s.status==OrderStatus.Delivered).Sum(p=>p.payment.Amount))
                }).OrderByDescending(c=>c.TotalSpent).Take(limit).ToList();
            return topCustomers;

        }
    }
}
