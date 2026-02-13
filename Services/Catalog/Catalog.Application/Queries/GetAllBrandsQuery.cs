using Catalog.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Queries
{
    public class GetAllBrandsQuery :IRequest<IList<BrandResponseDto>>
    {
    }
}
