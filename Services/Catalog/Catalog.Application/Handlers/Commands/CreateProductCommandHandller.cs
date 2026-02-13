using AutoMapper;
using Catalog.Application.Comand;
using Catalog.Application.Responses;
using Catalog.Core.Entites;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Handlers.Commands
{
    public class CreateProductCommandHandller : IRequestHandler<CreatePeoductCommand, ProductResponseDto>
    {  

         private readonly     IMapper _mapper;
        private readonly IProductRepositreis _productRepositreis;



        public CreateProductCommandHandller (IMapper mapper ,IProductRepositreis productRepositreis)
        {
            _mapper = mapper;
            _productRepositreis = productRepositreis;



        }




        async Task<ProductResponseDto> IRequestHandler<CreatePeoductCommand, ProductResponseDto>.Handle(CreatePeoductCommand request, CancellationToken cancellationToken)
        {

            var productEntity = _mapper.Map<Product>(  request);
        
            var NewProduct= await _productRepositreis.CreateProductAsync(productEntity);
            var productResponse =_mapper.Map<ProductResponseDto>(NewProduct);
            return productResponse;
        
        }
    }
}
