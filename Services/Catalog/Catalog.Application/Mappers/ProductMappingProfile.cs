using AutoMapper;
using Catalog.Application.Responses;
using Catalog.Core.Entites;
using Catalog.Core.Repositories;
using Catalog.Core.Spaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Mappers
{
    public class ProductMappingProfile :Profile
     {
      
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductResponseDto>().ReverseMap();
            CreateMap<productBrand, BrandResponseDto>().ReverseMap();
            CreateMap<productType, TypesResponseDto>().ReverseMap();
            CreateMap<Pagination <Product>, Pagination<ProductResponseDto>>().ReverseMap();
            
        }

    }
}
