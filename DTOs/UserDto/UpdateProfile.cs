namespace Ecommerce_ASP.NET.DTOs.UserDto
{
    public class UpdateProfile
    {
        public string password { get; set; }
        public string confirmePassword { get; set; }
        public string F_Name { get; set; }
        public string Email { get; set; }
        public string L_Name { get; set; }
        public string phone { get; set; }
        public DateTime create_Update { get; set; }
        public UpdateProfile()
        {
            create_Update = DateTime.Now;
        }
    }
}
