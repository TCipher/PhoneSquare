using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public interface IcatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
