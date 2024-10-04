using Application.DTOs;
using Application.Interfaces;

namespace Application.UseCases.Products
{
    public class GetProductsByCategoryId
    {
        private readonly IProductRepository _productRepository;

        public GetProductsByCategoryId(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> ExecuteAsync(int categoryId)
        {
            var products = await _productRepository.GetByCategoryIdAsync(categoryId);

            return products.Select(product => new ProductDto
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Image = product.Image
            });
        }
    }
}
