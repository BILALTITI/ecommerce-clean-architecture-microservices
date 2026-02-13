using Basket.Application.Commands;
using Basket.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.Handllers.Commands
{
    public class DeleteShoppingCartByUserNameCommandHandller : IRequestHandler<DeleteShoppingCartByUserNameCommand, Unit>
    {
        private readonly IBasketRepository _basketRepository;

        public DeleteShoppingCartByUserNameCommandHandller(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        async Task<Unit> IRequestHandler<DeleteShoppingCartByUserNameCommand, Unit>.Handle(DeleteShoppingCartByUserNameCommand request, CancellationToken cancellationToken)
        {
            await _basketRepository.DeleteBasket(request.userName);
            return Unit.Value;
        }
    }
}
