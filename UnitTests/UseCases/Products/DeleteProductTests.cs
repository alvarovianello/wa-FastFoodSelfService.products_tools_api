using Application.Interfaces;
using Application.UseCases.Products;
using Domain.Entities;
using Moq;

namespace UnitTests.UseCases.Products
{
    public class DeleteProductTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly DeleteProduct _deleteProduct;

        public DeleteProductTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _deleteProduct = new DeleteProduct(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange
            int productId = 1;

            // Simula que o produto não foi encontrado no repositório
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync((Product)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _deleteProduct.ExecuteAsync(productId));

            Assert.Equal("Produto não encontrado.", exception.Message);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldCallDeleteAsync_WhenProductExists()
        {
            // Arrange
            int productId = 1;
            var existingProduct = new Product { Id = productId, Name = "Product", Price = 10, CategoryId = 1 };

            // Simula que o produto existe no repositório
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(existingProduct);

            // Act
            await _deleteProduct.ExecuteAsync(productId);

            // Assert
            _productRepositoryMock.Verify(repo => repo.DeleteAsync(productId), Times.Once);
        }
    }
}
