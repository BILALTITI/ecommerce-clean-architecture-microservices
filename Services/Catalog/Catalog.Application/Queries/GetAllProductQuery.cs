using Catalog.Application.Responses;
using Catalog.Core.Spaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Queries
{
    public class GetAllProductQuery : IRequest<Pagination<ProductResponseDto>>
    {
        public CatalogSpecParam spec;

        public GetAllProductQuery (CatalogSpecParam spec)
        {

            this.spec = spec;
        }
    }
}
