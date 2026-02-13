using Discount.Application.Commands;
using Discount.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Discount.Application.Handllers.Commands
{
    public class DeleteDiscountCommandHandller : IRequestHandler<DeleteDiscountCommand, bool>
    {
        private readonly IdiscountRepository _IdiscountRepository;

        public DeleteDiscountCommandHandller(IdiscountRepository idiscountRepository)
        {
           _IdiscountRepository = idiscountRepository;
        }

        public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            var deleted = await _IdiscountRepository.DeleteDiscount(request.ProductName);


            return deleted;
                
                
                }
    }
}
