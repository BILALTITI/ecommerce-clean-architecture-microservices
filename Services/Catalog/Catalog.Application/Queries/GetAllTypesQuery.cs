using Catalog.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Queries
{
    public class GetAllTypesQuery  : IRequest<IList<TypesResponseDto>>
    {
    }
}
