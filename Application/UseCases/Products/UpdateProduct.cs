using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases.Products
{
    public class UpdateProduct
    {
        private readonly IProductRepository _productRepository;

        public UpdateProduct(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task ExecuteAsync(ProductDto productDto)
        {

            var existingProduct = await _productRepository.GetByIdAsync(productDto.Id);
            if (existingProduct == null)
            {
                throw new Exception("Produto não encontrado.");
            }

            var product = new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                CategoryId = productDto.CategoryId,
                Image = productDto.Image
            };
            await _productRepository.UpdateAsync(product);
        }
    }
}
