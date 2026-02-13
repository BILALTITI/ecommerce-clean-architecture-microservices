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
    public class GetProductByNameQueryHandller : IRequestHandler<GetProductByNameQuery, IList<ProductResponseDto>>
    {

        private readonly IMapper _mapper;

        private readonly IProductRepositreis  _repositreis;

        public GetProductByNameQueryHandller(IMapper mapper, IProductRepositreis repositreis)
        {
            _mapper = mapper;
            _repositreis = repositreis;
        }
        public async Task<IList<ProductResponseDto>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
         
       var products =await _repositreis.GetProductByNameAsync(request.Name);
            var result = _mapper.Map <IList<ProductResponseDto>>(products) ;
            return result ;

        }
    }
}
