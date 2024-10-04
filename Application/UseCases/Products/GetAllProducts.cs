using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases.Products
{
    public class GetAllProducts
    {
        private readonly IProductRepository _productRepository;

        public GetAllProducts(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> ExecuteAsync(string productIds)
        {
            IEnumerable<Product> products;

            if (!string.IsNullOrEmpty(productIds))
            {
                var ids = productIds.Split(',').Select(id => int.TryParse(id, out var result) ? result : (int?)null)
                                    .Where(id => id.HasValue)
                                    .Select(id => id.Value)
                                    .ToArray();
                products = await _productRepository.GetByFilterAsync(ids);
            }
            else
            {
                products = await _productRepository.GetAllAsync();
            }

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
