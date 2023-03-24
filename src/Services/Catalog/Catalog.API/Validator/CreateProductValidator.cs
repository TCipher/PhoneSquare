using Catalog.API.Data;
using Catalog.API.Data.DTOs;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using FluentValidation;
using MongoDB.Driver;
using System;

namespace Catalog.API.Validator
{
    public class CreateProductValidator : AbstractValidator<CreateProductDto>
    {

        //public ProductRepository(IcatalogContext context)
        //{
        //    //perform a null check on the context class
        //    _context = context ?? throw new ArgumentException(nameof(context));
        //}
        private readonly IProductRepository _productRepository;


        public CreateProductValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentException(nameof(productRepository));

            RuleFor(x => x.Name)
                 .NotEmpty().WithMessage("{PropertyName} is required.")
                  .Must(BeUniqueName)
                  .WithMessage("Name '{PropertyValue}' already exists.")
                 .NotNull()
                 .MaximumLength(150).WithMessage("{PropertyName} must not exceed 50 characters.");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                 .NotNull()
                 .MaximumLength(250).WithMessage("{PropertyName} must not exceed 250 characters.");
            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                 .NotNull()
                 .MaximumLength(150).WithMessage("{PropertyName} must not exceed 450 characters.");
            RuleFor(x => x.Summary)
               .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(450).WithMessage("{PropertyName} must not exceed 450 characters.");
            RuleFor(x => x.ImageFile)
              .NotEmpty().WithMessage("{PropertyName} is required.")
               .NotNull();
             

            RuleFor(x => x.Price)
                 .NotEmpty().WithMessage("{PropertyName} is required.")
                 .GreaterThan(0);
                
        }
        private bool BeUniqueName(CreateProductDto dto, string productName)
        {
            // Check if Product already exists in database
            var filter = Builders<Product>.Filter.Eq(p => p.Name, productName);
            var count = _productRepository.GetCollection().CountDocuments(filter);

            // If product is being created, name should not exist in database
            return count == 0;
        }
    }
}
