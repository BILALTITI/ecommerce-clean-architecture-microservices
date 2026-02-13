using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.Commands
{
    public class DeleteShoppingCartByUserNameCommand : IRequest<Unit>
    {

        // Unit Dosent Retern Any Thing using with MediatR to represent a void return type is similar to void in C# build in struct 

        public string userName { get; set; }
        public DeleteShoppingCartByUserNameCommand(string UserName)
        { 

                    

            userName = UserName;

        }
    }
}

