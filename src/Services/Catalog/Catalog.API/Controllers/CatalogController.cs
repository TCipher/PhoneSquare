using Catalog.API.Data.DTOs;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Catalog.API.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductService productService, ILogger<CatalogController> logger)
        {
            _productService = productService ?? throw new ArgumentException(nameof(productService));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>>GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return  Ok(products);
        }
        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDto>> GetProductById(string id)
        {
            //Retrieve the product
            var Product = await _productService.GetProductAsync(id);
            if(Product == null)
            {
                _logger.LogError($"Product with id: {id}, not found. ");
                return NotFound();  
            }
            return Ok(Product);

        }
      

        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductByCategory(string category)
        {
            var products = await _productService.GetProductByCategoryAsync(category);
            return Ok(products);
        }

        [Route("[action]/{name}", Name = "GetProductByName")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductByName(string name)
        {
            var prodName = await _productService.GetProductByNameAsync(name);
            return Ok(prodName);
        }

        [HttpPost]
        public async Task<ActionResult<CreateProductDto>> CreateProduct([FromBody] CreateProductDto CreateProductDto)
        {
            var result = await _productService.CreateProductAsync(CreateProductDto);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto updateProduct)
        {
            return Ok(await _productService.UpdateProductAsync(updateProduct));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await _productService.DeleteProductAsync(id));
        }
    }
}


