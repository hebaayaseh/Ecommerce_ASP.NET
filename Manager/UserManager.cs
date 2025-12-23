using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.User;
using Ecommerce_ASP.NET.DTOs.UserDto;
using Ecommerce_ASP.NET.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_ASP.NET.Manager
{
    public class UserManager
    {
        private readonly AppDbContext dbContext;
        private readonly UpdateProfile updateProfile;
        public UserManager(AppDbContext dbContext , UpdateProfile updateProfile)
        {
            this.dbContext = dbContext;
            this.updateProfile = updateProfile;
        }
        public User? GetProfile(int userId)
        {
            
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
        public User? UpdateProfile(int userId ,UpdateProfile userdto)
        {
            var user = dbContext.Users.Where(u => u.id == userId).FirstOrDefault();
            if(user == null) return null;
            
                user.f_name = userdto.F_Name ;
                user.l_name = userdto.L_Name ;
                user.email = userdto.Email ;
                user.phone = userdto.phone ;
                dbContext.SaveChanges();
                return user;
        }

    }
}
