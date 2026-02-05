using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_ASP.NET.Models
{
    public class OrderTrackings
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Orders")]
        public int OrderId { get; set; }
        public Orders Orders { get; set; }
        public string CurrentStatus { get; set; }
        public DateTime LastUpdated { get; set; }
        public List<TrackingHistories> TrackingHistories { get; set; }
    }
}
