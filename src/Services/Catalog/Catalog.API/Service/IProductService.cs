using AspNetCoreHero.Results;
using Catalog.API.Data.DTOs;
using Catalog.API.Entities;

namespace Catalog.API.Service
{
    public interface IProductService
    {
        Task<Result<List<ProductDto>>>GetProductsAsync();
        Task<Result<ProductDto>>GetProductAsync(string id);
        Task<Result<IEnumerable<ProductDto>>>GetProductByNameAsync(string name);
        Task<Result<IEnumerable<ProductDto>>>GetProductByCategoryAsync(string categoryName);

        Task<Result<CreateProductDto>>CreateProductAsync(CreateProductDto createProductDto);
        Task<Result<UpdateProductDto>> UpdateProductAsync(UpdateProductDto updateProductDto);
        Task<Result<string>> DeleteProductAsync(string id);
    }
}
