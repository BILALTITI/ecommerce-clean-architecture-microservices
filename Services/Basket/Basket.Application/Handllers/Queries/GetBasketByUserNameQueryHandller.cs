using AutoMapper;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.Handllers.Queries
{
    public class GetBasketByUserNameQueryHandller : IRequestHandler<GetBasketByUserNameQuery, ShoppingCartResponse>
    {

        private readonly IMapper _Mapper;   

        private readonly IBasketRepository _basketRepository;
        public GetBasketByUserNameQueryHandller(IMapper mapper, IBasketRepository basketRepository)
        {
           _Mapper = mapper;
            _basketRepository = basketRepository;
        }
        public async Task<ShoppingCartResponse> Handle(GetBasketByUserNameQuery request, CancellationToken cancellationToken)
        { 
         
            var SHoppingCart  =   await   _basketRepository.GetBasket(request.UserName);

            var response = _Mapper.Map<ShoppingCartResponse>(SHoppingCart);
            return response;

        }
    }
}
