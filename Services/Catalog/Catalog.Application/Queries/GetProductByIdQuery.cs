using Catalog.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Queries
{
    public class GetProductByIdQuery : IRequest<ProductResponseDto>

    {
        public string id { get; set; }

        public GetProductByIdQuery(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                throw new ArgumentNullException(nameof(Id), "Id cannot be null or empty.");
            }
            id = Id;
        }
    }
}
