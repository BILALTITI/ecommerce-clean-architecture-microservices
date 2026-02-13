using Basket.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.Responses
{
    public class ShoppingCartResponse
    {
        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();


        public ShoppingCartResponse() 
        
        {
        
        }
        public ShoppingCartResponse(string usernaeme)
        {

            UserName = usernaeme;

        }

        public decimal TotalPrice{

            get
            {
                            
                decimal totalPrice = 0;
                foreach (var item in Items)
                {
                    totalPrice += item.Price * item.Quantity;
                }
                return totalPrice;

            }

        }
    }
}
