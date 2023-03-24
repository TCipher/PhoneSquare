using AspNetCoreHero.Results;
using AutoMapper;
using Catalog.API.Data.DTOs;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Catalog.API.Validator;
using FluentValidation;
using MongoDB.Driver;

namespace Catalog.API.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _productRepository = productRepository ?? throw new ArgumentException(nameof(productRepository));
        }

        public async Task<Result<CreateProductDto>> CreateProductAsync(CreateProductDto CreateproductDto)
        {
            var validator = new CreateProductValidator(_productRepository);
            var validationResult = await validator.ValidateAsync(CreateproductDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            Product product = _mapper.Map<Product>(CreateproductDto);
            await _productRepository.CreateProductAsync(product);
            var result = _mapper.Map<CreateProductDto>(product);
            if(result == null)
            {
                return await Result<CreateProductDto>.FailAsync("An Error occured while creating the Product");
            }

            return new Result<CreateProductDto> { Succeeded = true, Data = result, Message =" The product was created successfully"};
        }

        public async Task<Result<string>> DeleteProductAsync(string id)
        {
            //check if the Id is null
            if (id == null)
            {
                return await Result<string>.FailAsync("The Id does not exist in the databse");
            }
          
             await _productRepository.DeleteProductAsync(id);
            
            return await Result<string>.SuccessAsync("The Product was deleted Successfully");

        }

        public async Task<Result<ProductDto>> GetProductAsync(string id)
        {
            var product = await _productRepository.GetProductAsync(id);
            var RetrievedProduct = _mapper.Map<ProductDto>(product);
            //check if the Id is null
            if (id == null)
            {
                return await Result<ProductDto>.FailAsync("The Id does not exist in the databse");
            }
            if (RetrievedProduct == null)
            {
                return await Result<ProductDto>.FailAsync("The product does not exist in the databse");
            }

            return new Result<ProductDto> { Succeeded = true, Data = RetrievedProduct, Message = "The Product was Retrieved Successfully" };
        }

        public async Task<Result<IEnumerable<ProductDto>>> GetProductByCategoryAsync(string categoryName)
        {
            //check if the categoryName is null
            if (categoryName == null)
            {
                return new Result<IEnumerable<ProductDto>> { Succeeded = false, Data = null, Message = "Invalid categoryName" };
            }
           
              var product = await _productRepository.GetProductByCategoryAsync(categoryName);
            var prodCat = _mapper.Map<List<ProductDto>>(product);
            //Check to see if the retrieved categoryName  is null
            if (prodCat == null)
            {
                return new Result<IEnumerable<ProductDto>> { Succeeded = false, Data = null, Message = "Failed to retieve the categoryName" };
            }
          
          
            return new Result<IEnumerable<ProductDto>>{ Succeeded = true, Data = prodCat, Message = "Successful" };
        }

            public async Task<Result<IEnumerable<ProductDto>>> GetProductByNameAsync(string name)
        {
            //check if the name is null
            if (name == null)
            {
                return new Result<IEnumerable<ProductDto>>{ Succeeded = false, Data = null, Message = "Invalid name" };
            }
            var product = await _productRepository.GetProductByNameAsync(name);
            var prodName = _mapper.Map<List<ProductDto>>(product);
            //Check to see if the retrieved name is null
            if (prodName == null)
            {
                return new Result<IEnumerable<ProductDto>> { Succeeded = false, Data = null, Message = "Failed to retieve the name" };
            }

            return new Result<IEnumerable<ProductDto>> { Succeeded = true, Data = prodName, Message = "Successful" };
        }

        public async Task<Result<List<ProductDto>>> GetProductsAsync()
        {
            var products = await _productRepository.GetProductsAsync();
            var allProducts = _mapper.Map<List<ProductDto>>(products);

            if(allProducts.Count == 0)
            {
                return new Result<List<ProductDto>>{Succeeded = false, Data = null, Message = "Failed to retieve the products" };
            }
            return new Result<List<ProductDto>>{ Succeeded = true, Data = allProducts, Message = "Successfull" };
        }
        

       
            //Get the product 
        public async Task<Result<UpdateProductDto>> UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            try
            {
                if (updateProductDto == null)
                {
                    return await Result<UpdateProductDto>.FailAsync("The product does not exist in the databse");
                }
               Product product = _mapper.Map<Product>(updateProductDto);
                 var updateProd =   await _productRepository.UpdateProductAsync(product);
                return await Result<UpdateProductDto>.SuccessAsync("The product was Updated Successfully");
            }
            catch (Exception)
            {

                return new Result<UpdateProductDto>{ Succeeded = false, Message = "An Unexpected Error Occured" };
            }
           


        }
    }
}
