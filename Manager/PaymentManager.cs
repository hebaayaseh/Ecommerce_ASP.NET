
using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.Payment;
using Ecommerce_ASP.NET.Models;
using Ecommerce_ASP.NET.Models.Enums;

namespace Ecommerce_ASP.NET.Manager
{
    public class PaymentManager
    {
        readonly AppDbContext dbContext;
        private readonly BankApprove bankApprove;
        public PaymentManager(BankApprove bankApprove,AppDbContext dbContext)
        {
            this.bankApprove = bankApprove;
            this.dbContext = dbContext;
        }
        public PaymentStatus processPayment(Orders orders)
        {
            
            bool approved = bankApprove.bankApprove(orders.totalPrice);

            if (!approved)
                return PaymentStatus.Completed;

            return PaymentStatus.Completed;
        }
        public List<PaymentDto>? GetPaymentMethod()
        {
            var methods = dbContext.payments.Select(
                o=>new PaymentDto
                {
                    method=o.PaymentMethod.ToString()
                }).ToList();
            return methods;
        }
    }
}
