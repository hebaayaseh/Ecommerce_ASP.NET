using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Dashboard;
using Ecommerce_ASP.NET.DTOs.Order;

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
        
    }
}
