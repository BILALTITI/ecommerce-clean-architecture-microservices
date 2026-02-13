using AutoMapper;
using EventBusMessages.Events;
using MassTransit;
 using MediatR;
using Ordering.Application.Commands;
using System.Security.AccessControl;

namespace Ordering.API.EventBusConsumer
{
    public class BaksetOrderingConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
     private readonly ILogger<BaksetOrderingConsumer> _logger;

        public BaksetOrderingConsumer(IMapper mapper, IMediator mediator, ILogger<BaksetOrderingConsumer> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            using var scope = _logger.BeginScope("consuming basket checkout event for {correlationid}", context.Message.CreatingId);
            var cmd =   _mapper.Map<CheckoutOrderCommand>(context.Message);
            var result = await _mediator.Send(cmd);
            _logger.LogInformation("Basket checkout event completed !!");
        }
    }
}
