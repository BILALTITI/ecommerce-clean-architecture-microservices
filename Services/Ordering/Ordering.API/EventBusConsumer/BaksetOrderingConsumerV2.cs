using AutoMapper;
using EventBusMessages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Commands;

namespace Ordering.API.EventBusConsumer
{

        public class BaksetOrderingConsumerV2 : IConsumer<BasketCheckoutEventV2>
        {
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;
            private readonly ILogger<BaksetOrderingConsumerV2> _logger;

            public BaksetOrderingConsumerV2(IMapper mapper, IMediator mediator, ILogger<BaksetOrderingConsumerV2> logger)
            {
                _mapper = mapper;
                _mediator = mediator;
                _logger = logger;
            }
            public async Task Consume(ConsumeContext<BasketCheckoutEventV2> context)
            {
                using var scope = _logger.BeginScope("consuming basket checkout event for {correlationid} From Version 2", context.Message.CreatingId);
                var cmd = _mapper.Map<CheckoutOrderCommandV2>(context.Message);
                var result = await _mediator.Send(cmd);
                _logger.LogInformation("Basket checkout event completed  From Version 2 !!");
            }
        }

    
}
