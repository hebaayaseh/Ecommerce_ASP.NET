
using Ecommerce_ASP.NET.Models;
using Ecommerce_ASP.NET.Models.Enums;

namespace Ecommerce_ASP.NET.Manager
{
    public class ProcessPayment
    {
        private readonly BankApprove bankApprove;
        public ProcessPayment(BankApprove bankApprove)
        {
            this.bankApprove = bankApprove;
        }
        public PaymentStatus processPayment(Orders orders)
        {
            // 1. Get order amount
            // 2. Send payment request (mock)
            
            bool approved = bankApprove.bankApprove(orders.totalPrice);

            if (!approved)
                return PaymentStatus.Completed;

            return PaymentStatus.Completed;
        }

    }
}
