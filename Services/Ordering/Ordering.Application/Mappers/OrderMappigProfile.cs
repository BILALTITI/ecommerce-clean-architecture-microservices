using AutoMapper;
using EventBusMessages.Events;
using Ordering.Application.Commands;
using Ordering.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Mappers
{
    public class OrderMappigProfile     :Profile
    {
        public OrderMappigProfile() {
        
        


            CreateMap<Core.Entites.Order, Application.Responses.OrderResponse>().ReverseMap();
            CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
            CreateMap<Order, UpdateOrderCommand>().ReverseMap();
            CreateMap<CheckoutOrderCommand, BasketCheckoutEvent>().ReverseMap();





        }
    }
}
