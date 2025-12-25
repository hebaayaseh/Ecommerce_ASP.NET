

namespace Ecommerce_ASP.NET.Manager
{
    public class BankApprove
    {
        public bool bankApprove(decimal amount)
        {
            if (amount > 0) return true;
            return false;
        }
    }
}
