using Discount.Grpc.Protos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Discount.Application.Commands
{
    public class CreateDiscountCommand: IRequest<CouponModel>
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        //public CreateDiscountCommand(string productName, string description, int amount)
        //{
        //    ProductName = productName;
        //    Description = description;
        //    Amount = amount;
        //}
    }
}
