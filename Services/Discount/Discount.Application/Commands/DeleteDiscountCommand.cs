using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Discount.Application.Commands
{
    public class DeleteDiscountCommand :IRequest<bool>
    {
        public string ProductName { get; set; }

        public DeleteDiscountCommand(string productName)
        {
            ProductName = productName;
        }
    }
}
