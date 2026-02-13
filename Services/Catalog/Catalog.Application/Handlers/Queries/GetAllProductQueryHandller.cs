using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Core.Spaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Handlers.Queries
{
    public class GetAllProductQueryHandller : IRequestHandler<GetAllProductQuery, Pagination<ProductResponseDto>>
    {
        private readonly IProductRepositreis _productRepository;
        private readonly IMapper _mapper;   

        public GetAllProductQueryHandller(IProductRepositreis productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<Pagination<ProductResponseDto>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {

            var products = await _productRepository.GetAllProductAsync(request.spec);
            var productDtos = _mapper.Map<Pagination<ProductResponseDto>>(products);

            return productDtos;
        }
    }
}
