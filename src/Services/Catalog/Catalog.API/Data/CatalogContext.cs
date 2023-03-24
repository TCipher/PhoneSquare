using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : IcatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            //create mongo client for connection to the mongo database
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            //Creating the database
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            //populate the product collection
            Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));

            //seed data into database
            CatalogContextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products { get; }
    }
}
