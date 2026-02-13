using Catalog.Application.Comand;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Handlers.Commands
{
   public class DeleteProductCommandHandller : IRequestHandler<DeleteProductCommand, bool>
    {


        private readonly IProductRepositreis _productRepository;
      
        public DeleteProductCommandHandller(IProductRepositreis productRepository)
        {
            _productRepository = productRepository;
        }       

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        { 
        
        return await _productRepository.DeleteProductAsync(request.Id); 



        }
    }
}
