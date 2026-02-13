using Catalog.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Queries
{
    public class GetProductByBrandNameQuery :IRequest<IList<ProductResponseDto>>
    {
        public string BrandName { get; set; }

        public GetProductByBrandNameQuery(string brandName)
        {
            if (string.IsNullOrWhiteSpace(brandName))
            {
                throw new ArgumentNullException(nameof(brandName), "BrandName cannot be null or empty.");
            }
            BrandName = brandName;
        }
    }
}
