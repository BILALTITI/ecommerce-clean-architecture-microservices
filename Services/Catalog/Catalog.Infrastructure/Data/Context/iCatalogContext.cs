using Catalog.Core.Entites;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data.Context
{
    public interface iCatalogContext
    {
        IMongoCollection<Product> Products { get; }
        IMongoCollection<productBrand> productBrands { get; }
        IMongoCollection<productType> productTypes { get; }
    }
}