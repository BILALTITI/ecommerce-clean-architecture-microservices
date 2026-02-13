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
    public class DeleteOrderCommandHandller : IRequestHandler<DeleteOrderCommand, Unit>
    {
        private readonly IOrderReposirtoy _orderRepository;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;


        public DeleteOrderCommandHandller(IOrderReposirtoy orderRepository, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {

            var OrderToDelete = await _orderRepository.GetByIdAsync(request.Id);

            if (OrderToDelete == null)
            {
                _logger.LogWarning(
                    "Delete failed. Order with Id {OrderId} was not found.",
                    request.Id
                );

                throw new OrderNotFoundException(nameof(Order), request.Id);
            }

            await _orderRepository.DeleteAsync(OrderToDelete);

            _logger.LogInformation(
                "Order with Id {OrderId} deleted successfully.",
                request.Id
            );

            return Unit.Value;

        }
    }
}
