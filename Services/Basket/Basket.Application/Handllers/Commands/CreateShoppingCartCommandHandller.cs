using AutoMapper;
using Basket.Application.Commands;
using Basket.Application.GrpcServices;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.Handllers.Commands
{
    public class CreateShoppingCartCommandHandller : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
    {

        private readonly IMapper _mapper;

        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IBasketRepository _basketRepository;

        public CreateShoppingCartCommandHandller(DiscountGrpcService discountGrpcService , IMapper mapper, IBasketRepository basketRepository)
        {
            _discountGrpcService = discountGrpcService;
            _mapper = mapper;
            _basketRepository = basketRepository;
        }
        public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {     
         
             foreach(var item in request.Items)
             {
                 var coupon = await _discountGrpcService.GetDiscountAsync(item.ProductName);
                if (coupon is not null )
                 item.Price -= coupon.Amount;
             }


            var ShoppingCarts = await _basketRepository.UpdateBasket(new ShoppingCart()
            {
                UserName = request.UserName,
                Items = request.Items,
            });

            var response = _mapper.Map<ShoppingCartResponse>(ShoppingCarts);
            return response;
        }
    }
}
