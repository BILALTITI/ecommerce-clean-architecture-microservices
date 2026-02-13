using Catalog.Core.Entites;
using Catalog.Core.Spaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Core.Repositories
{
    public interface IProductRepositreis
    {
        Task<Pagination<Product>> GetAllProductAsync(CatalogSpecParam catalogSpecParam);
        Task<Product> GetProductByIdAsync(string id);

        Task<IEnumerable<Product>> GetProductByNameAsync(string name);
        Task<IEnumerable<Product>> GetProductByBrandNameAsync(string Brandname);

        Task <Product> CreateProductAsync (Product product);
        Task <bool> UpdateProductAsync (Product product);
        Task <bool>DeleteProductAsync (string id);
    }
}
