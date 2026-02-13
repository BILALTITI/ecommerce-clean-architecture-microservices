using Discount.Grpc.Protos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Discount.Application.Commands
{
    public class UpdateDiscountCommand :IRequest<CouponModel>
    {


        public int id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
    }
}
