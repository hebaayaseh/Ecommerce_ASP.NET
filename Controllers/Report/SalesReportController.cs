using Ecommerce_ASP.NET.Manager.Excel;
using Ecommerce_ASP.NET.Manager.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_ASP.NET.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesReportController : ControllerBase
    {
        public readonly ReportManager reportManager;
        public readonly ExportSalesReportToExcel exportSalesReportToExcel;
        public SalesReportController(ReportManager reportManager)
        {
            this.reportManager = reportManager;
        }
        [Authorize(Roles ="Admin")]
        [HttpGet("ExportSalesReportExcel")]
        public IActionResult ExportSalesReportExcel(DateTime from, DateTime to)
        {
            var report = reportManager.SalesReport(from, to);
            var file = exportSalesReportToExcel.ExportSalesReportExcel(report);

            return File(
                file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "SalesReport.xlsx"
            );
        }
    }
}
