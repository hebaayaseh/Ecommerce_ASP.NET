namespace Ecommerce_ASP.NET.DTOs.Order
{
    public class TrackingHistoryDto
    {
        public DateTime updateAt { get; set; }
        public string status { get; set; }
        public string Notes { get; set; }
        public TrackingHistoryDto()
        {
            updateAt = DateTime.Now;
        }
    }
}
