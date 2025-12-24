namespace Ecommerce_ASP.NET.DTOs.Product
{
    public class GetProduct
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int stock { get; set; }
        public string image { get; set; }
       
    }
}
