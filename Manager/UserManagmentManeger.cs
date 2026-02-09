using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Discount;
using Ecommerce_ASP.NET.DTOs.UserManagment;

namespace Ecommerce_ASP.NET.Manager
{
    public class UserManagmentManeger
    {
        public readonly AppDbContext dbContext;
        public UserManagmentManeger(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<UserDto>? GetAllUser(int page = 5, int pageSize = 10)
        {
            var user = dbContext.Users.Skip((page - 1) * pageSize)
                .Take(pageSize).Select(u => new UserDto
                {
                    Id = u.id,
                    Name = u.f_name + " " + u.l_name
                }).ToList();
            return user;
        }
        public UserDto? GetUserById(int userId)
        {
            var user = dbContext.Users.Where(u => u.id == userId)
                .Select(u => new UserDto
                {
                    Id = u.id,
                    Name = u.f_name + " " + u.l_name
                }).FirstOrDefault();
            return user;
        }
        public UserDto? DeletUserById(int userId)
        {
            var user = dbContext.Users.Where(u => u.id == userId).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            dbContext.Users.Remove(user);
            dbContext.SaveChanges();
            return new UserDto
            {
                Id = user.id,
                Name = user.f_name + " " + user.l_name
            };
        }
        public UserDto? SearchUser(string str)
        {
            var user = dbContext.Users.Where(u => u.f_name.Contains(str) || u.l_name.Contains(str))
                .Select(u => new UserDto
                {
                    Id = u.id,
                    Name = u.f_name + " " + u.l_name
                }).FirstOrDefault();
            return user;
        }
    }
}
