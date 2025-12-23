using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Category;
using Ecommerce_ASP.NET.Models;
using Ecommerce_ASP.NET.Models.Enums;

namespace Ecommerce_ASP.NET.Manager
{
    public class CategoryManager
    {
        private readonly AppDbContext dbContext;
        public CategoryManager(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void AddCategory(AddCategory addCategory , int userId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin && u.id == userId);
            if (user == null) throw new UnauthorizedAccessException("Only Admin Can Add Products!");
            if ( addCategory.name == null || addCategory.description == null)
            {
                throw new ArgumentNullException("Category details cannot be null");
            }
           
            var newcategory =  new Categories
               {
                name = addCategory.name,
                description = addCategory.description,
               };
                dbContext.Categories.Add(newcategory);
                dbContext.SaveChanges();
            
        }
        public Categories SearchCategory(string name , int userId)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.role == UserRole.Admin && u.id == userId || u.role == UserRole.Customer && u.id == userId);
            if (user == null) throw new UnauthorizedAccessException("Please Login!");
            var category = dbContext.Categories.FirstOrDefault(c => c.name.Contains(name));
            if (category == null) throw new KeyNotFoundException("Category Not Found!");
            return category;
        }
    }
}
