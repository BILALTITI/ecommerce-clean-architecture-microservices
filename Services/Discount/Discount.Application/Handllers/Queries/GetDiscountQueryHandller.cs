using AutoMapper;
using Discount.Application.Queries;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Discount.Application.Handlers.Queries
{
    public class GetDiscountQueryHandler
        : IRequestHandler<GetDiscountQuery, CouponModel>
    {
        private readonly IdiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public GetDiscountQueryHandler(
            IMapper mapper,
            IdiscountRepository discountRepository)
        {
            _mapper = mapper;
            _discountRepository = discountRepository;
        }

        public async Task<CouponModel> Handle(
            GetDiscountQuery request,
            CancellationToken cancellationToken)
        {
            var coupon = await _discountRepository
                .GetDiscount(request.ProductName);

            if (coupon == null)
            {
                throw new RpcException(
                    new Status(
                        StatusCode.NotFound,
                        $"Discount for product '{request.ProductName}' not found"));
            }
            var CouponModel = new CouponModel{
            Id=coupon.Id,
            ProductName=coupon.ProductName,
            Description=coupon.Description,
            Amount=coupon.Amount
            


            };
            return _mapper.Map<CouponModel>(coupon);
        }
    }
}
