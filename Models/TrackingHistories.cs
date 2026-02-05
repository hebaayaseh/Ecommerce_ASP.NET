using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_ASP.NET.Models
{
    public class TrackingHistories
    {
        [Key]
        public int Id { get; set; }
        public DateTime UpdateAt { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        [ForeignKey("OrderTrackings")]
        public int OrderTrackingId { get; set; }
        public OrderTrackings OrderTrackings { get; set; }
        public TrackingHistories()
        {
            UpdateAt = DateTime.Now;
        }
    }
}
