using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_ASP.NET.Controllers.Dashboard
{
    [Route("api/[controller]")]
    [ApiController]
    public class adminDashboardController : ControllerBase
    {
        public readonly adminDashboardManager adminDashboardManager;
        public adminDashboardController(adminDashboardManager adminDashboardManager)
        {
            this.adminDashboardManager = adminDashboardManager;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetOverView")]
        public IActionResult GetOverView()
        {
            var overview = adminDashboardManager.GetOverView();
            if (overview == null)
            {
                return NotFound("No data found for the dashboard overview.");
            }
            return Ok(overview);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("OrderByStatus")]
        public IActionResult OrderByStatus()
        {
            var orderStatus = adminDashboardManager.OrderByStatus();
            if (orderStatus == null || !orderStatus.Any())
            {
                return NotFound("No data found for orders by status.");
            }
            return Ok(orderStatus);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("ProductsByCategory")]
        public IActionResult ProductsByCategory()
        {
            var productsByCategory = adminDashboardManager.productsByCategories();
            if (productsByCategory == null || !productsByCategory.Any())
            {
                return NotFound("No data found for products by category.");
            }
            return Ok(productsByCategory);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("SalesStatistics")]
        public IActionResult SalesStatistics([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (from > to)
            {
                return BadRequest("The 'from' date must be earlier than the 'to' date.");
            }
            var salesStatistics = adminDashboardManager.SalesStatistics(from, to);
            if (salesStatistics == null)
            {
                return NotFound("No sales data found for the specified date range.");
            }
            return Ok(salesStatistics);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("TopSellingProducts")]
        public IActionResult TopSellingProducts()
        {
            var topSellingProducts = adminDashboardManager.GetTopSellingProducts();
            if (topSellingProducts == null || !topSellingProducts.Any())
            {
                return NotFound("No data found for top-selling products.");
            }
            return Ok(topSellingProducts);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetTopCustomers")]
        public IActionResult GetTopCustomers()
        {
            var topCustomers = adminDashboardManager.GetTopCustomers();
            if (topCustomers == null || !topCustomers.Any())
            {
                return NotFound("No data found for customer demographics.");
            }
            return Ok(topCustomers);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetRevenueChartMonthly")]
        public IActionResult GetRevenueChartMonthly()
        {
            var revenueChart = adminDashboardManager.GetRevenueMonthlyChart();
            if (revenueChart == null || !revenueChart.Any())
            {
                return NotFound("No data found for monthly revenue chart.");
            }
            return Ok(revenueChart);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetRevenueChartDaily")]
        public IActionResult GetRevenueChartDaily()
        {
            var revenueChart = adminDashboardManager.GetRevenueChartDaily();
            if (revenueChart == null || !revenueChart.Any())
            {
                return NotFound("No data found for daily revenue chart.");
            }
            return Ok(revenueChart);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetOrdersChartMonthly")]
        public IActionResult GetOrdersChartMonthly()
        {
            var ordersChart = adminDashboardManager.GetOrdersChartMontly;
            if (ordersChart == null)
            {
                return NotFound("No data found for monthly orders chart.");
            }
            return Ok(ordersChart);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetOrdersChartDaily")]
        public IActionResult GetOrdersChartDaily()
        {
            var ordersChart = adminDashboardManager.GetOrdersChartDaily;
            if (ordersChart == null)
            {
                return NotFound("No data found for daily orders chart.");
            }
            return Ok(ordersChart);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetCategoryDistribution")]
        public IActionResult GetCategoryDistribution()
        {
            var categoryDistribution = adminDashboardManager.GetCategoryDistribution();
            if (categoryDistribution == null)
            {
                return NotFound("No data found for category distribution.");
            }
            return Ok(categoryDistribution);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetRecentOrders")]
        public IActionResult GetRecentOrders()
        {
            var recentOrders = adminDashboardManager.GetRecentOrders();
            if (recentOrders == null || !recentOrders.Any())
            {
                return NotFound("No data found for recent orders.");
            }
            return Ok(recentOrders);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetLowStockProducts")]
        public IActionResult GetLowStockProducts()
        {
            var lowStockProducts = adminDashboardManager.GetLowStockProducts();
            if (lowStockProducts == null || !lowStockProducts.Any())
            {
                return NotFound("No data found for low stock products.");
            }
            return Ok(lowStockProducts);
        }
    }
}
