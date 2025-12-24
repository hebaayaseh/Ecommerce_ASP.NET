using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Discount;
using Ecommerce_ASP.NET.Models;
using Ecommerce_ASP.NET.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_ASP.NET.Manager
{
    public class DiscountManager
    {
        private readonly AppDbContext dbContext;
        public DiscountManager(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Discount AddDiscountCode(DiscountDto discountDto, int userId)
        {
            var admin = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin && u.id == userId);
            if (admin == null) throw new UnauthorizedAccessException("Only Admin Can Enter Discount Code!");
            var discount = new Discount()
            {
                Code = discountDto.code,
                Type = discountDto.discountType,
                DiscountValue = discountDto.amount,
                EndDate = discountDto.ExpiryDate,
                MaxUsage = discountDto.MaxUsage,
                minimumOrderAmount = discountDto.minimumOrderAmount,
            };
            dbContext.discounts.Add(discount);
            dbContext.SaveChanges();
            return discount;
        }
        public void UpdateDiscount(DiscountDto discountDto, int userId)
        {
            var admin = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin && u.id == userId);
            if (admin == null) throw new UnauthorizedAccessException("Only Admin Can Update Discount Code!");
            var discount = dbContext.discounts.FirstOrDefault(d => d.Id == discountDto.id);
            if (discount == null) throw new KeyNotFoundException("Discount Code Not Found!");

            discount.Code = discountDto.code;
            discount.Type = discountDto.discountType;
            discount.DiscountValue = discountDto.amount;
            discount.EndDate = discountDto.ExpiryDate;
            discount.MaxUsage = discountDto.MaxUsage;
            discount.minimumOrderAmount = discountDto.minimumOrderAmount;

            dbContext.SaveChanges();
        }
        public void DeleteDiscount(int discountId, int userId)
        {
            var admin = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin && u.id == userId);
            if (admin == null) throw new UnauthorizedAccessException("Only Admin Can Delete Discount Code!");
            var discount = dbContext.discounts.FirstOrDefault(d => d.Id == discountId);
            if (discount == null) throw new KeyNotFoundException("Discount Code Not Found!");
            dbContext.discounts.Remove(discount);
            dbContext.SaveChanges();
        }
    }
}
