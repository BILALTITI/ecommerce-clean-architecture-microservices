using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Application.Handlers.Commands;
using Ordering.Core.Entites;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Handllers.Commands
{
    public class UpdateOrderCommandHandllers : IRequestHandler<UpdateOrderCommand, Unit>
    {
        private readonly IOrderReposirtoy _orderRepository;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public UpdateOrderCommandHandllers(IOrderReposirtoy orderRepository, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {

            var OrderToUpdate = await _orderRepository.GetByIdAsync(request.Id);

            if (OrderToUpdate == null)
            {
                _logger.LogWarning(
                    "Update failed. Order with Id {OrderId} was not found.",
                    request.Id
                );

                throw new OrderNotFoundException(nameof(Order), request.Id);
            }

            await _orderRepository.UpdateAsync(OrderToUpdate);

            _logger.LogInformation(
                "Order with Id {OrderId} updated successfully.",
                request.Id
            );


            return Unit.Value;
        }
    }
}
