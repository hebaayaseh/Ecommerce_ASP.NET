using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.Models;

namespace Ecommerce_ASP.NET.Manager
{
    public class UserManager
    {
        private readonly AppDbContext dbContext;
        public UserManager(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public User? GetProfile(int userId)
        {
            //var user = dbContext.Users.FirstOrDefault(u=>u.id == userId);
            var user = dbContext.Users
                .Where(u => u.id == userId)
                .Select(u => new User
                {
                    id = u.id,
                    f_name = u.f_name,
                    l_name = u.l_name,
                    email = u.email,
                    phone = u.phone,
                    created_at = u.created_at,
                    updated_at = u.updated_at
                })
                .FirstOrDefault();
            if (user == null) return null;
            else return user;
        }
    }
}
