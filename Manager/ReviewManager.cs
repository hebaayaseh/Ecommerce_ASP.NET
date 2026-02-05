using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Review;
using Ecommerce_ASP.NET.Models;

namespace Ecommerce_ASP.NET.Manager
{
    public class ReviewManager
    {
        public readonly AppDbContext dbContext;
        public ReviewManager(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void AddReview(ReviewDto review)
        {
            var newReview = new Review()
            {
                Id = review.id,
                ProductId = review.productId,
                UserId = review.userId,
                Rating = review.rating,
                Comment = review.comment,
                CreatedAt = review.DateTime

            };
            dbContext.reviews.Add(newReview);
            dbContext.SaveChanges();
        }
        public List<ReviewDto>? GetProductReview(int productTd)
        {
            var reviews = dbContext.reviews
                .Where(p => p.ProductId == productTd)
                .Select(r => new ReviewDto
                {
                    id = r.Id,
                    comment = r.Comment,
                    productId = r.ProductId,
                    rating = r.Rating,
                    userId = r.UserId,
                    DateTime = r.CreatedAt

                }).ToList();

            return reviews;
        }
        public ReviewDto UpdateReview(ReviewDto newreview)
        {
            var updateReview = dbContext.reviews.Where(r => r.Id == newreview.id)
                .Select(r => new ReviewDto
                {
                    id = r.Id,
                    comment = r.Comment,
                    productId = r.ProductId,
                    rating = r.Rating,
                    userId = r.UserId,
                    DateTime = r.CreatedAt
                }).FirstOrDefault();
            dbContext.SaveChanges();
            if (updateReview == null) throw new KeyNotFoundException("Review not found");
            return updateReview;
        }
        public void DeleteReview(int reviewId)
        {
            var review = dbContext.reviews.FirstOrDefault(r => r.Id == reviewId);
            if (review == null) throw new KeyNotFoundException("Review not found");
            dbContext.reviews.Remove(review);
            dbContext.SaveChanges();
        }
        public List<ReviewDto>? GetMyReview(int userId)
        {
            var reviews = dbContext.reviews.Where(u => u.UserId == userId)
                .Select(r => new ReviewDto
                {
                    id = r.Id,
                    comment = r.Comment,
                    productId = r.ProductId,
                    rating = r.Rating,
                    userId = r.UserId,
                    DateTime = r.CreatedAt
                }).ToList();
            return reviews;
        }
        public double GetAverageRating(int productId)
        {
            var review = dbContext.reviews
             .Where(r => r.ProductId == productId)
             .Select(r => (double?)r.Rating)
             .Average();
            return review ?? 0;

        }
        public List<ReviewDto>? GetAllReviews()
        {
            var reviews = dbContext.reviews
                .Select(r => new ReviewDto
                {
                    id = r.Id,
                    comment = r.Comment,
                    productId = r.ProductId,
                    rating = r.Rating,
                    userId = r.UserId,
                    DateTime = r.CreatedAt
                }).ToList();
            return reviews;
        }
        public void AdminDeleteReviews(int reviewId)
        {
            var reviews = dbContext.reviews.Where(r => r.ProductId == reviewId).ToList();
            dbContext.reviews.RemoveRange(reviews);
            dbContext.SaveChanges();
        }

    }
}
