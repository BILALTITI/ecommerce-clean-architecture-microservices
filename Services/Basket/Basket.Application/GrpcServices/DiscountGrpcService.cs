using Discount.Grpc.Protos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        {
            _discountProtoServiceClient = discountProtoServiceClient ?? throw new ArgumentNullException(nameof(discountProtoServiceClient));
        }

        public async Task<CouponModel> GetDiscountAsync(string productName)
        {
            var request = new GetDiscountRequest { ProductName = productName };
            return await _discountProtoServiceClient.GetDiscountAsync(request);
        }


        public async Task<CouponModel> CreateDiscountAsync(CouponModel coupon)
        {
            var request = new CreateDiscountRequest { Coupon = coupon };
            return await _discountProtoServiceClient.CreateDiscountAsync(request);
        }

        public async Task<CouponModel> UpdateDiscountAsync(CouponModel coupon)
        {
            var request = new UpdateDiscountRequest { Coupon = coupon };
            return await _discountProtoServiceClient.UpdateDiscountAsync(request);
        }

        public async Task<DeleteDiscountResponse> DeleteDiscountAsync(string productName)
        {
            var request = new DeleteDiscountRequest { ProductName = productName };
            return await _discountProtoServiceClient.DeleteDiscountAsync(request);
        }

    }
}
