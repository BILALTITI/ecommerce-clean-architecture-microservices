using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Commands
{
    public class CheckoutOrderCommandV2:IRequest<int>
    {
        public string UserName { get; set; }
        public decimal TotalaPrice { get; set; }


    }
}
