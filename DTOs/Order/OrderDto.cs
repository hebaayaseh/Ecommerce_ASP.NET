using Ecommerce_ASP.NET.DTOs.AddOrderItem;

namespace Ecommerce_ASP.NET.DTOs.Order
{
    public class OrderDto
    {
        public int id { get; set; }
        public decimal totalPrice { get; set; }
        public int UserId { get; set; }
        public List<OrderItemsDto> OrderItems { get; set; }
        public List<OrderTrackingDto> TrackingItems { get; set; }
        public string status { get; set; }
        public int? discountId { get; set; }
        public int AddressId { get; set; }
    }
}
