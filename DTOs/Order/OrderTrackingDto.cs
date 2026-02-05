namespace Ecommerce_ASP.NET.DTOs.Order
{
    public class OrderTrackingDto
    {
        public int OrderId { get; set; }
        public string CurrentStatus { get; set; }
        public DateTime LastUpdated { get; set; }
        public List<TrackingHistoryDto> History { get; set; }
    }
}
