namespace Ecommerce_ASP.NET.DTOs.Invoice
{
    public class InvoiceItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public double subTotal { get; set; }
    }
}
