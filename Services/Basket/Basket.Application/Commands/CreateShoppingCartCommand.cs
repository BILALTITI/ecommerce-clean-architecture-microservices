using Basket.Application.Responses;
using Basket.Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.Commands
{
    public class CreateShoppingCartCommand:IRequest<ShoppingCartResponse>
    {
        public string UserName { get; set; }

        public List<ShoppingCartItem> Items { get; set; }
    }
}
