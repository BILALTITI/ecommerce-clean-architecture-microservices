using Catalog.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Queries
{
    public class GetProductByNameQuery :IRequest <IList < ProductResponseDto>>
    {
        public string Name { get; set; }

        public GetProductByNameQuery(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");
            }
            Name = name;
        }
    }
}
