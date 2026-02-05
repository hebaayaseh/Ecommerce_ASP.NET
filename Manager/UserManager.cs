using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Address;
using Ecommerce_ASP.NET.DTOs.Discount;
using Ecommerce_ASP.NET.DTOs.Product;
using Ecommerce_ASP.NET.DTOs.User;
using Ecommerce_ASP.NET.DTOs.UserDto;
using Ecommerce_ASP.NET.Helpers;
using Ecommerce_ASP.NET.Models;
using Ecommerce_ASP.NET.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_ASP.NET.Manager
{
    public class UserManager
    {
        private readonly AppDbContext dbContext;
        private readonly UpdateProfile updateProfile;
        private readonly PasswordHasher passwordHasher;
        public UserManager(AppDbContext dbContext , UpdateProfile updateProfile,PasswordHasher passwordHasher)
        {
            this.dbContext = dbContext;
            this.updateProfile = updateProfile;
            this.passwordHasher = passwordHasher;

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
     
        public decimal GetPrice(int productId)
        {
            var product = dbContext.Products.Where(p => p.id == productId).FirstOrDefault();
            if (product == null) throw new KeyNotFoundException("Not Found Product!");
            return product.price;
        }
        public void changePassword(int userId, string currentPassword, string newPassword)
        {
            var user =dbContext.Users.FirstOrDefault(u=> u.id==userId);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");
            if (!passwordHasher.Verify(currentPassword, user.passwordHash))
                throw new Exception("Current password is incorrect");
            user.passwordHash = passwordHasher.Hash(newPassword);
            user.updated_at = DateTime.Now;
            dbContext.SaveChanges();
        }
        public AddressDto GetAddress(int userId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.id == userId);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");
            
            var address = dbContext.addresses.FirstOrDefault(a=>a.userId == userId);
            if (address == null)
                throw new KeyNotFoundException("Address not found");
            return new AddressDto
                {
                id = address.id,
                street = address.Street,
                city = address.City,
                postalCode = address.PostalCode,
                building = address.building,
                PhoneNumber = address.PhoneNumber
            };
        }
        public void AddAddress(int userId, AddressDto addressDyo)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.id == userId);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");
            var address = new Address
            {
                Street = addressDyo.street,
                City = addressDyo.city,
                PostalCode = addressDyo.postalCode,
                building = addressDyo.building,
                PhoneNumber = addressDyo.PhoneNumber,
                userId = userId
            };
            dbContext.addresses.Add(address);
            dbContext.SaveChanges();
        }
        public void UpdateAddress(int userId , AddressDto addressDto)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.id == userId);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");
            var address = dbContext.addresses.FirstOrDefault(a => a.userId == userId);
            if (address == null)
                throw new KeyNotFoundException("Address not found");
            address.Street = addressDto.street;
            address.City = addressDto.city;
            address.PostalCode = addressDto.postalCode;
            address.building = addressDto.building;
            address.PhoneNumber = addressDto.PhoneNumber;
            dbContext.SaveChanges();
        }
    }
}
