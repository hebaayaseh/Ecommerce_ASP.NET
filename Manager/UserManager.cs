using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Address;
using Ecommerce_ASP.NET.DTOs.Discount;
using Ecommerce_ASP.NET.DTOs.Order;
using Ecommerce_ASP.NET.DTOs.Product;
using Ecommerce_ASP.NET.DTOs.User;
using Ecommerce_ASP.NET.DTOs.UserDto;
using Ecommerce_ASP.NET.Models;
using Ecommerce_ASP.NET.Models.Enums;
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
        public ICollection<User> GetAllUsers(int adminId)
        {
            var admin = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin && u.id == adminId);
            if (admin == null) throw new UnauthorizedAccessException("Only Admin Can View All Users!");
            var user =dbContext.Users
                .Select(u => new User
                {
                    id = u.id,
                    f_name = u.f_name,
                    l_name = u.l_name,
                    email = u.email,
                    phone = u.phone,
                    role = u.role,
                    created_at = u.created_at,
                    updated_at = u.updated_at
                })
                .ToList();
            return user;
        }
        /*public Orders createOrder(AddOrder order , int userId)
        {
            var admin = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin && u.id == userId || u.role == UserRole.Customer && u.id == userId);
            if (admin == null) throw new UnauthorizedAccessException("Please Login!");
            var order1 = new Orders()
            {
                UserId = userId,
                AddressId = order.Address.id,
                discountId = order.DiscountCode != null ? order.DiscountCode.id : null,
                OrderItems = order.addOrderItem.Select(oi => new OrderItems
                {
                    ProductId = oi.productToOrder.productId,
                    quantity = oi.productToOrder.quantity,
                    price = GetPrice(oi.productToOrder.productId)
                }).ToList(),
                created_at = DateTime.Now
            };*/


           
        public decimal GetPrice(int productId)
        {
            var product = dbContext.Products.Where(p => p.id == productId).FirstOrDefault();
            if (product == null) throw new KeyNotFoundException("Not Found Product!");
            return product.price;
        }

    }
}
