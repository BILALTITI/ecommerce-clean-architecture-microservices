using Catalog.Core.Entites;
using Catalog.Core.Repositories;
using Catalog.Core.Spaces;
using Catalog.Infrastructure.Data.Context;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepositoriy : IProductRepositreis, ITypeRepositreis, IBrandRepositreis
    {
        public iCatalogContext _context { get; set; }
        public ProductRepositoriy(iCatalogContext context)
        {
            _context = context;
        }
        public async Task<Product> GetProductByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id), "Id cannot be null or empty.");
            }
            
            // Validate ObjectId format (must be exactly 24 hex characters)
            if (id.Length != 24 || !ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException($"Invalid ObjectId format: '{id}'. ObjectId must be exactly 24 hexadecimal characters. Received {id.Length} characters.", nameof(id));
            }
            
            // Use ObjectId in filter to ensure proper comparison
            var filter = Builders<Product>.Filter.Eq("_id", objectId);
            return await _context.Products.Find(filter).FirstOrDefaultAsync();

        }

        public async Task<Pagination<Product>> GetAllProductAsync(CatalogSpecParam catalogSpecParam)
        {
            var filter = Builders<Product>.Filter.Empty;
            
            // Apply search filter if provided
            if (!string.IsNullOrEmpty(catalogSpecParam.Search))
            {
                filter = Builders<Product>.Filter.Regex(p => p.Name, new BsonRegularExpression(catalogSpecParam.Search, "i"));
            }
            
            // Apply brand filter if provided
            if (!string.IsNullOrEmpty(catalogSpecParam.BrandId))
            {
                filter = filter & Builders<Product>.Filter.Eq(p => p.Brand.Id, catalogSpecParam.BrandId);
            }
            
            // Apply type filter if provided
            if (!string.IsNullOrEmpty(catalogSpecParam.TypeId))
            {
                filter = filter & Builders<Product>.Filter.Eq(p => p.Type.Id, catalogSpecParam.TypeId);
            }
            
            var totalCount = await _context.Products.CountDocumentsAsync(filter);
            var products = await ApplyPaginationAsync(catalogSpecParam, filter);
            
            return new Pagination<Product>(
                catalogSpecParam.PageIndex,
                catalogSpecParam.PageSize,
                (int)totalCount,
                products
            );
        }
        public async Task<IEnumerable<productBrand>> GetAllBrandsAsync()
        { 
           return await _context.productBrands.Find(brand => true).ToListAsync();
        }

        public async Task<IEnumerable<productType>> GetAllTypesAsync()
        {
return  await _context.productTypes.Find(type => true).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByBrandNameAsync(string Brandname)
        {
            if (string.IsNullOrWhiteSpace(Brandname))
            {
                throw new ArgumentNullException(nameof(Brandname), "Brandname cannot be null or empty.");
            }
            return await _context.Products.Find(Product => Product.Brand.Name == Brandname).ToListAsync();
         }

        public async Task<IEnumerable<Product>> GetProductByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");
            }
            var product = await _context.Products.Find(Product => Product.Name == name).FirstOrDefaultAsync();
            return product != null ? new List<Product> { product } : new List<Product>();
        }
        public  async Task<Product> CreateProductAsync(Product product)

        {
            await _context.Products.InsertOneAsync(product);
            return product;
         }

        public async Task<bool> DeleteProductAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id), "Id cannot be null or empty.");
            }
            
            // Validate ObjectId format (must be exactly 24 hex characters)
            if (id.Length != 24 || !ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException($"Invalid ObjectId format: '{id}'. ObjectId must be exactly 24 hexadecimal characters. Received {id.Length} characters.", nameof(id));
            }
         
            // Use ObjectId in filter to ensure proper comparison
            var filter = Builders<Product>.Filter.Eq("_id", objectId);
            var DeleteResult = await _context.Products.DeleteOneAsync(filter);

            return DeleteResult.IsAcknowledged && DeleteResult.DeletedCount > 0;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.Id))
            {
                throw new ArgumentNullException(nameof(product), "Product or Product.Id cannot be null or empty.");
            }
            
            // Use filter builder to avoid ObjectId parsing issues
            var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
            var UpdatedProduct = await _context.Products.ReplaceOneAsync(filter, product);

            return UpdatedProduct.IsAcknowledged && UpdatedProduct.ModifiedCount > 0;

        }

        private async Task<IReadOnlyList<Product>>  ApplyPaginationAsync(CatalogSpecParam collection, FilterDefinition<Product> filter)
        { 


          var sortDefination =Builders<Product>.Sort.Ascending(p => p.Name);

            if(!string.IsNullOrEmpty(collection.Sort))
            {
                switch (collection.Sort)
                {
                    case "priceAsc":
                        sortDefination = Builders<Product>.Sort.Ascending(p => p.Price);
                        break;
                    case "priceDesc":
                        sortDefination = Builders<Product>.Sort.Descending(p => p.Price);
                        break;
                    default:
                        sortDefination = Builders<Product>.Sort.Ascending(p => p.Name);
                        break;
                }
            }
            return await _context.Products.Find(filter).Sort(sortDefination)
                .Skip((collection.PageIndex - 1) * collection.PageSize)
                .Limit(collection.PageSize)
                .ToListAsync();
        }

    }
}
