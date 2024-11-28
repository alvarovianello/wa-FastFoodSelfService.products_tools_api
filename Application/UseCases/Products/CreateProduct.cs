using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases.Products
{
    public class CreateProduct
    {
        private readonly IProductRepository _productRepository;

        public CreateProduct(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task ExecuteAsync(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                CategoryId = productDto.CategoryId,
                Image = productDto.Image
            };
            await _productRepository.AddAsync(product);
        }
    }
}
