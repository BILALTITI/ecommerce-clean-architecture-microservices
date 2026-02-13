using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entites;
using Ordering.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers.Commands
{
    public class CheckoutOrderCommandHandler
        : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IOrderReposirtoy _orderRepository;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(
            IOrderReposirtoy orderRepository,
            ILogger<CheckoutOrderCommandHandler> logger,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<int> Handle(
            CheckoutOrderCommand request,
            CancellationToken cancellationToken)
        {
            // Example flow (typical Clean Architecture)
            var orderEntity = _mapper.Map<Order>(request);

            var GetrateOrder = await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation("Order {OrderId} created successfully",GetrateOrder.Id);


            return GetrateOrder.Id;
        }
    }
}
