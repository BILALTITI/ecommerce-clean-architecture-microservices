using Basket.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.Queries
{
    public  class GetBasketByUserNameQuery:IRequest<ShoppingCartResponse>
    {
        public string UserName { get; set; }
        public GetBasketByUserNameQuery(string userName)
        {
            UserName = userName;
        }

    }
}
