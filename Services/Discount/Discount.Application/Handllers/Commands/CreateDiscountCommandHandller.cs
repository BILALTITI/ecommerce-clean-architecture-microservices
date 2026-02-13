using AutoMapper;
using Discount.Application.Commands;
using Discount.Core.Entites;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Discount.Application.Handllers.Commands
{
    public class CreateDiscountCommandHandller : IRequestHandler<CreateDiscountCommand, CouponModel>
    {
        private readonly IdiscountRepository _discountRepository;
        
        private readonly IMapper mapper;


        public CreateDiscountCommandHandller(IdiscountRepository discountRepository, IMapper mapper)
        {
            _discountRepository = discountRepository;
            this.mapper = mapper;
        }

        public async Task<CouponModel> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
     
        { 
        
        var coupon = mapper.Map<Coupon>(request);

            var newCoupon = await _discountRepository.CreateDiscount(coupon);
            var couponModel = mapper.Map<CouponModel>(newCoupon);


            return couponModel;

        }
    }
}
