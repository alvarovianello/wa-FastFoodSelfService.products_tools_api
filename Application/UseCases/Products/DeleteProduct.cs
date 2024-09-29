using Application.Interfaces;

namespace Application.UseCases.Products
{
    public class DeleteProduct
    {
        private readonly IProductRepository _productRepository;

        public DeleteProduct(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task ExecuteAsync(int id)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                throw new Exception("Produto não encontrado.");
            }
            await _productRepository.DeleteAsync(id);
        }
    }
}
