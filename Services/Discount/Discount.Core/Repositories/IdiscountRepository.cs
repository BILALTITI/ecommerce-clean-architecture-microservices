using System;
using System.Collections.Generic;
using System.Text;

namespace Discount.Core.Repositories
{
    public interface IdiscountRepository
    {
        Task<Entites.Coupon> GetDiscount(string productName);
        Task<bool> CreateDiscount(Entites.Coupon coupon);
        Task<bool> UpdateDiscount(Entites.Coupon coupon);
        Task<bool> DeleteDiscount(string productName);
    }
}
