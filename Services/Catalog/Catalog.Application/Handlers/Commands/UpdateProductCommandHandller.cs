using AutoMapper;
using Catalog.Application.Comand;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Handlers.Commands
{
    public class UpdateProductCommandHandller : IRequestHandler<UpdateProductCommand, bool>
    {

         private readonly IProductRepositreis _productRepositreis;

        public UpdateProductCommandHandller(  IProductRepositreis productRepos)
        {
             _productRepositreis = productRepos;
        }
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {

            var productEntity = await _productRepositreis.UpdateProductAsync(new Core.Entites.Product()
            {

                Id = request.Id,
                Name = request.Name,
                Brand = request.Brand,
                Summary = request.Summary,
                Type = request.Type,
                Price = request.Price,
                ImageFile = request.ImageFile,
                Description = request.Description,
               



            });

            return productEntity;

        
        }
 
    }
}
