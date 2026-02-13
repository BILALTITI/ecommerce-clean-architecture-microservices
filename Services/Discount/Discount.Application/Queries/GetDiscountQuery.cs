using Discount.Grpc.Protos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Discount.Application.Queries
{
    public class GetDiscountQuery : IRequest<CouponModel>
    {
        public string ProductName { get; set; }
        public GetDiscountQuery(string productName)
        {
            ProductName = productName;
        }
    }
}
