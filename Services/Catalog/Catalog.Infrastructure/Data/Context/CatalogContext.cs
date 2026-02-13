using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Core.Entites;
using Microsoft.Extensions.Configuration;
using Catalog.Infrastructure.Data.Contexts;
namespace Catalog.Infrastructure.Data.Context
{
    public class CatalogContext : iCatalogContext
    {
        public IMongoCollection<Product> Products {


            get;
        }

        public IMongoCollection<productBrand> productBrands { get; }
        public IMongoCollection<productType> productTypes { get; }
        public CatalogContext(IConfiguration configuration)
        {
        
        var connectionString = configuration["DatabaseSettings:ConnectionString"];
        var databaseName = configuration["DatabaseSettings:DatabaseName"];
        var productsCollectionName = configuration["DatabaseSettings:ProductsCollectionName"];
        var productBrandsCollectionName = configuration["DatabaseSettings:productBrandsCollectionName"];
        var productTypesCollectionName = configuration["DatabaseSettings:productTypesCollectionName"];

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "DatabaseSettings:ConnectionString cannot be null or empty.");
        }
        if (string.IsNullOrWhiteSpace(databaseName))
        {
            throw new ArgumentNullException(nameof(databaseName), "DatabaseSettings:DatabaseName cannot be null or empty.");
        }
        if (string.IsNullOrWhiteSpace(productsCollectionName))
        {
            throw new ArgumentNullException(nameof(productsCollectionName), "DatabaseSettings:ProductsCollectionName cannot be null or empty.");
        }
        if (string.IsNullOrWhiteSpace(productBrandsCollectionName))
        {
            throw new ArgumentNullException(nameof(productBrandsCollectionName), "DatabaseSettings:productBrandsCollectionName cannot be null or empty.");
        }
        if (string.IsNullOrWhiteSpace(productTypesCollectionName))
        {
            throw new ArgumentNullException(nameof(productTypesCollectionName), "DatabaseSettings:productTypesCollectionName cannot be null or empty.");
        }

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        Products = database.GetCollection<Product>(productsCollectionName);
        productBrands = database.GetCollection<productBrand>(productBrandsCollectionName);
        productTypes = database.GetCollection<productType>(productTypesCollectionName);

         _=BrandContextSeed.SeedDataAsync(productBrands);
            _= TypeContextSeed.SeedDataAsync(productTypes);

           _=  ProductContextSeed.SeedDataAsync(Products);
        }
    



    }
}
