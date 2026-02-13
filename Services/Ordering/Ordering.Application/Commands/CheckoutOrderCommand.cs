using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Commands
{
    public class CheckoutOrderCommand:IRequest<int>
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }


        public string? FIrstName { get; set; }


        public string? LastName { get; set; }

        public string? EmailAddress { get; set; }


        public string? AddressLine { get; set; }


        public string? Country { get; set; }

        public string? State { get; set; }

        public string? ZipCode { get; set; }

        public string? PaymentMethod { get; set; }
        public string? CardNumber { get; set; }

        public string? CardHolderName { get; set; }
        public string? CVV { get; set; }
        public DateTime? CardExpiration { get; set; }
    }
}
