using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entites;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Handllers.Commands
{
    public class CheckoutOrderCommandV2Handller:IRequestHandler<CheckoutOrderCommandV2,int>
    {
        private readonly IMapper _mapper;
        private readonly IOrderReposirtoy _orderRepository;
        private readonly ILogger<CheckoutOrderCommandV2Handller> _logger;
        public CheckoutOrderCommandV2Handller(IMapper mapper, IOrderReposirtoy orderRepository, ILogger<CheckoutOrderCommandV2Handller> logger)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _logger = logger;
        }
        public async Task<int> Handle(CheckoutOrderCommandV2 request, CancellationToken cancellationToken)
        {
            // Example flow (typical Clean Architecture)
            var orderEntity = _mapper.Map<Order>(request);

            var GetrateOrder = await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation("Order {OrderId} created successfully From Version 2", GetrateOrder.Id);


            return GetrateOrder.Id;
        }
    }
}
