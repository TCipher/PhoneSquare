using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.Validator;
using MongoDB.Driver;
using System.Xml.Linq;

namespace Catalog.API.Repositories
{
    public class ProductRepository: IProductRepository
    {
        //inject the Icatalog Context
        private readonly IcatalogContext _context;
        public ProductRepository(IcatalogContext context)
        {
            //perform a null check on the context class
            _context = context ?? throw new ArgumentException(nameof(context)); 
        }

        public async Task CreateProductAsync(Product product)
        {
          
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            // filter definition for querying MongoDB database
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult  = await _context
                                    .Products
                                   .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public IMongoCollection<Product> GetCollection()
        {
            return _context.Products;
        }

        public async Task<Product> GetProductAsync(string id)
        {
            var product = await _context
                                    .Products
                                    .Find(p => p.Id == id)
                                    .FirstOrDefaultAsync();
            return product;
        }

        public async Task<IEnumerable<Product>> GetProductByCategoryAsync(string categoryName)
        {
            // filter definition for querying MongoDB database
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

            var productCategoryName = await _context
                                    .Products
                                    .Find(filter)
                                    .ToListAsync();
            return productCategoryName;
        }

        public  async Task<IEnumerable<Product>> GetProductByNameAsync(string name)
        {
            // filter definition for querying MongoDB database
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            var productByname = await _context
                                    .Products
                                    .Find(filter)
                                    .ToListAsync();
            return productByname; 
;

        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
           
            var AllProducts = await _context
                                .Products
                                .Find(p => true)
                                .ToListAsync();
            return AllProducts;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
           var UpdateProduct = await _context
                                      .Products
                                      .ReplaceOneAsync(filter: x => x.Id == product.Id, replacement: product);

            return UpdateProduct.IsAcknowledged
                && UpdateProduct.ModifiedCount > 0;
                
        }
    }
}
