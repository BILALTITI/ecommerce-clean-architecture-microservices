using AutoMapper;
using MediatR;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Entites;
using Ordering.Core.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handllers.Queries
{
    public class GetOrdersListHandllers
        : IRequestHandler<GetOrderListQueries, List<OrderResponse>>
    {
        private readonly IOrderReposirtoy _orderRepository;
        private readonly IMapper _mapper;

        public GetOrdersListHandllers(
            IMapper mapper,
            IOrderReposirtoy orderRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public GetOrdersListHandllers(IOrderReposirtoy orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public Task<List<OrderResponse>> Handle(
            GetOrderListQueries request,
            CancellationToken cancellationToken)
        {
 
        
        
            var orders = _orderRepository.GetOrdersByUserName(request.UserName).Result;
            var ordersResponse = _mapper.Map<List<OrderResponse>>(orders);
            return Task.FromResult(ordersResponse);
        }
    }
}
