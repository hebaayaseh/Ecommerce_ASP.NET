using Ecommerce_ASP.NET.DTOs.Report;
using System.ComponentModel;
using OfficeOpenXml;
using OfficeOpenXml;
namespace Ecommerce_ASP.NET.Manager.Excel
{
    public class ExportSalesReportToExcel
    {
        public byte[] ExportSalesReportExcel(SalesReportDto report)
        {
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Sales Report");

            sheet.Cells[1, 1].Value = "Date";
            sheet.Cells[1, 2].Value = "Orders";
            sheet.Cells[1, 3].Value = "Revenue";

            int row = 2;
            foreach (var day in report.DailySales)
            {
                sheet.Cells[row, 1].Value = day.Date.ToString("yyyy-MM-dd");
                sheet.Cells[row, 2].Value = day.OrdersCount;
                sheet.Cells[row, 3].Value = day.Revenue;
                row++;
            }

            row += 2;
            sheet.Cells[row, 1].Value = "Total Orders";
            sheet.Cells[row, 2].Value = report.TotalOrders;

            row++;
            sheet.Cells[row, 1].Value = "Total Revenue";
            sheet.Cells[row, 2].Value = report.TotalRevenue;

            return package.GetAsByteArray();
        }
    }
}
