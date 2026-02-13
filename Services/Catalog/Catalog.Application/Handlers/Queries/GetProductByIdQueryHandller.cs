using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Handlers.Queries
{
    public class GetProductByIdQueryHandller : IRequestHandler<GetProductByIdQuery, ProductResponseDto>
    {

        private readonly IMapper _Mapper;
        private readonly IProductRepositreis _productRepositreis;
        public GetProductByIdQueryHandller(IMapper mapper ,IProductRepositreis productRepositreis)
        {
            _Mapper = mapper;
            _productRepositreis = productRepositreis;
        }
        public async Task<ProductResponseDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {

      var product = await  _productRepositreis.GetProductByIdAsync(request.id);
      var productResponse= _Mapper.Map<ProductResponseDto>(product);
            return productResponse;
        }
    }
}
