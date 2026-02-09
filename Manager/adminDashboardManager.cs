using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Dashboard;
using Ecommerce_ASP.NET.DTOs.SalesStatisticsDto;
using Ecommerce_ASP.NET.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_ASP.NET.Manager
{
    public class adminDashboardManager
    {
        public readonly AppDbContext dbContext;
        public adminDashboardManager(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            
        }
        public AdminDashboardDto GetOverView()
        {
            int totalUsers = dbContext.Users.Count();
            var totalProducts = dbContext.Products.Count();
            int totalOrders = dbContext.Orders.Where(o => o.status == OrderStatus.Delivered).Count();
            decimal totalRevenue = dbContext.payments.Where(p => p.Status == PaymentStatus.Completed).Sum(p => p.Amount);
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
                .GroupBy(p => new { p.categoryId, p.category.name })
                .Select(p => new ProductsByCategoryDto
                {
                    categoryName = p.Key.ToString(),
                    productcount = p.Count()
                }).ToList();
            return productsByCategory;
        }
        public SalesStatisticsDto SalesStatistics(DateTime from, DateTime to)
        {
            var fromDate = from.Date;
            var toDate = to.Date.AddDays(1).AddTicks(-1);
            var totalRevenue = dbContext.payments.Where(d => d.PaymentDate >= fromDate && d.PaymentDate <= toDate && d.Status == PaymentStatus.Completed).Sum(p => p.Amount);
            var totalOrders = dbContext.Orders.Where(o => o.created_at >= fromDate && o.created_at <= toDate && o.status == OrderStatus.Delivered).Count();
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
                .Where(o => o.Order.status == OrderStatus.Delivered)
                .GroupBy(p => p.ProductId)
                .Select(p => new GetTopSellingProductsDto
                {
                    Quantity = p.Sum(q => q.quantity),
                    ProductId = p.Key,
                });
            var topProducts = TopSellingProducts.OrderByDescending(p => p.Quantity).Take(limit).ToList();
            return topProducts;
        }
        public List<GetTopCustomersDto> GetTopCustomers(int limit = 10)
        {
            var topCustomers = dbContext.payments.Include(o => o.orders)
                .Where(p => p.Status == PaymentStatus.Completed)
                .GroupBy(u => u.orders.UserId)
                .Select(u => new GetTopCustomersDto
                {
                    CustomerId = u.Key,
                    TotalSpent = u.Sum((p => p.Amount))
                })
                .OrderByDescending(c => c.TotalSpent)
                .Take(limit).ToList();
            return topCustomers;

        }
        public List<GetRevenueChartMonthlyDto> GetRevenueMonthlyChart()
        {
            var totalRevenue = dbContext.payments.Where(p => p.Status == PaymentStatus.Completed)
                .GroupBy(p => (new { p.PaymentDate.Year, p.PaymentDate.Month }))
                .Select(r => new GetRevenueChartMonthlyDto
                {
                    month = new DateTime(DateTime.Now.Year, r.Key.Month, 1),
                    totalRevenue = r.Sum(p => p.Amount)
                }).ToList();
            return totalRevenue;
        }
        public List<GetRevenueChartDailyDto> GetRevenueChartDaily()
        {
            var totalRevenue = dbContext.payments.Where(p => p.Status == PaymentStatus.Completed)
                .GroupBy(p => p.PaymentDate.Date)
                .Select(r => new GetRevenueChartDailyDto { 
                   day =r.Key,
                   totalRevenue= r.Sum(p => p.Amount)
                }).ToList();
            return totalRevenue;
        }
        public List<GetOrdersChartMonthlyDto> GetOrdersChartMontly()
        {
            var totalOrders = dbContext.Orders.Where(o => o.status == OrderStatus.Delivered)
                .GroupBy(o => new { o.created_at.Year, o.created_at.Month })
                .Select(r => new GetOrdersChartMonthlyDto
                {
                    month = new DateTime(r.Key.Year, r.Key.Month, 1),
                    totalOrders = r.Count()
                }).ToList();
            return totalOrders;
        }
        public int GetOrdersChartDaily()
        {
            var totalOrders = dbContext.Orders.Where(o => o.status == OrderStatus.Delivered)
                .Where(o => o.created_at.Date == DateTime.Now.Date && o.created_at.Year == DateTime.Now.Year)
                .GroupBy(o => o.created_at.Date)
                .Select(r => new GetOrdersChartDailyDto
                {
                    Day = r.Key,
                    totalOrders = r.Count()
                });
            return totalOrders.Count();
        }
        public List<GetCategoryDistributionDto>? GetCategoryDistribution()
        {
            var productCount = dbContext.Products
                .GroupBy(c=>new { c.categoryId,c.category.name })
                .Select(c=> new GetCategoryDistributionDto
                {
                    CategoryName = c.Key.name,
                    productCount = c.Count(),
                    CategoryId = c.Key.categoryId   
                }).ToList();
            return productCount;
        }
        public List<GetOrdersChartDailyDto>? GetRecentOrders(int limit = 10)
        {
            var recentOrders = dbContext.Orders.Where(o=>o.status==OrderStatus.Delivered)
                .OrderByDescending(o => o.created_at)
                .Take(limit)
                .Select(o => new GetOrdersChartDailyDto
                {
                    Day = o.created_at,
                    totalOrders = 1
                }).ToList();
            return recentOrders;
        }
        public List<GetLowStockProductsDto>? GetLowStockProducts(int threshold = 10)
        {
            var products=dbContext.Products.Where(p=>p.stock<=threshold)
                .Select(p=> new GetLowStockProductsDto
                {
                    id = p.id,
                    name = p.name,
                    stock = p.stock
                }).ToList();
            return products;
        }
    }
}
