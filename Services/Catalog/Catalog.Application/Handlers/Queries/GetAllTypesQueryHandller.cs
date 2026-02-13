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
    public class GetAllTypesQueryHandller : IRequestHandler<GetAllTypesQuery, IList<TypesResponseDto>>
    {

        private readonly IMapper _mapper;
        private readonly ITypeRepositreis _typeRepository;
        public GetAllTypesQueryHandller(IMapper mapper, ITypeRepositreis typeRepository)
        {
            _mapper = mapper;
            _typeRepository = typeRepository;
        }
        public async Task<IList<TypesResponseDto>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
  
        var TypesList = await _typeRepository .GetAllTypesAsync();
            var result = _mapper.Map<IList<TypesResponseDto>>(TypesList).ToList();
            return result;
        }
    }
}
