using AutoMapper;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBusMessages.Events;

namespace Basket.Application.Mapper
{
    public class BasketMappingProfile:Profile
    {

        public BasketMappingProfile()
        {
           
            CreateMap<ShoppingCart, ShoppingCartResponse>().ReverseMap();
            CreateMap<ShoppingCartItem, ShoppingCartItemResponse>().ReverseMap();
        
        CreateMap<BasketCheckOut, BasketCheckoutEvent>().ReverseMap();
        CreateMap<BasketCheckoutV2, BasketCheckoutEventV2>().ReverseMap();
        }
    }
}
