using Application.DTOs;
using Application.Interfaces;

namespace Application.UseCases.Products
{
    public class GetProductById
    {
        private readonly IProductRepository _productRepository;

        public GetProductById(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> ExecuteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null) return null; // Retorna null se não encontrar

            return new ProductDto
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Image = product.Image
            };
        }
    }
}
