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
    public class GetAllBrandQueryHandller : IRequestHandler<GetAllBrandsQuery, IList<BrandResponseDto>>
    {

        private readonly IMapper _mapper;
        private readonly IBrandRepositreis _BrandRepositry;

        public GetAllBrandQueryHandller(IBrandRepositreis BrandRepositry, IMapper mapper)
        {
            _BrandRepositry = BrandRepositry;
            _mapper = mapper;
        }
        public async Task<IList<BrandResponseDto>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var BramdList = await _BrandRepositry.GetAllBrandsAsync();
            var BrandDtoList = _mapper.Map<IList<BrandResponseDto>>(BramdList.ToList());
            return BrandDtoList;    

        }
    }
}
