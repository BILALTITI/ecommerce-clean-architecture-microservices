using System;
using System.Collections.Generic;
using System.Text;

namespace Discount.Core.Entites
{
    public class Coupon
    {
        public static object Logging { get; set; }
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
    }
}
