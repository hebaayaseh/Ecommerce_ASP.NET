namespace Ecommerce_ASP.NET.DTOs.WishlistItems
{
    public class WishlistItemsDto
    {
        public int id { get; set; }
        public DateTime AddedAt { get; set; }
        public int userId { get; set; }
        public int productId { get; set; }

        public WishlistItemsDto() 
        {  
            AddedAt = DateTime.UtcNow;
        }

    }
}
