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
    internal class GetProductByBrandNameQueryHandller : IRequestHandler<GetProductByBrandNameQuery, IList<ProductResponseDto>>
    {


        private readonly IMapper _mapper;

        private readonly IProductRepositreis _repositreis;



        public GetProductByBrandNameQueryHandller(IProductRepositreis repositreis, IMapper mapper)
        {
            _repositreis = repositreis;
            _mapper = mapper;
        }

        public async Task<IList<ProductResponseDto>> Handle(GetProductByBrandNameQuery request, CancellationToken cancellationToken)
        {
            var products =await  _repositreis.GetProductByBrandNameAsync(request.BrandName);
            var productsDto = _mapper.Map<IList<ProductResponseDto>>(products).ToList();
            return  productsDto ;
        }
    }
 
}
