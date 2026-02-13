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
    public class UpdateDiscountCommandHandller : IRequestHandler<UpdateDiscountCommand, CouponModel>
    {

  private readonly       IMapper _mapper;

        private readonly IdiscountRepository _discountrepo;
        
        public UpdateDiscountCommandHandller(IMapper mapper, IdiscountRepository discountrepo)
        {

            _mapper = mapper;
            _discountrepo = discountrepo;

        }
        public async Task<CouponModel> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
        {





            var coupon = _mapper.Map<Coupon>(request);

            var newCoupon = await _discountrepo.UpdateDiscount(coupon);
            var couponModel = _mapper.Map<CouponModel>(newCoupon);


            return couponModel;




        }
    }
}
