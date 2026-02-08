namespace Ecommerce_ASP.NET.DTOs.Dashboard
{
    public class GetTopCustomersDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
