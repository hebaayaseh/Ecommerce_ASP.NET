namespace Ecommerce_ASP.NET.DTOs.Invoice
{
    public class InvoiceDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<InvoiceItemDto> Items { get; set; }
    }
}
