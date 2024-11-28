using Application.DTOs;
using Application.Interfaces;
using Application.UseCases.Products;
using Moq;

namespace UnitTests.UseCases.Products
{
    public class UpdateProductTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly UpdateProduct _updateProduct;

        public UpdateProductTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _updateProduct = new UpdateProduct(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldUpdateProduct_WhenProductExists()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Id = 1,
                Name = "Updated Product",
                Description = "Updated description",
                Price = 30.5m,
                CategoryId = 2,
                Image = "updated_image.png"
            };

            var existingProduct = new Domain.Entities.Product
            {
                Id = 1,
                Name = "Old Product",
                Description = "Old description",
                Price = 20.5m,
                CategoryId = 1,
                Image = "old_image.png"
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingProduct);
            _productRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Product>())).Returns(Task.CompletedTask);

            // Act
            await _updateProduct.ExecuteAsync(productDto);

            // Assert
            _productRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Domain.Entities.Product>(product =>
                product.Id == productDto.Id &&
                product.Name == productDto.Name &&
                product.Description == productDto.Description &&
                product.Price == productDto.Price &&
                product.CategoryId == productDto.CategoryId &&
                product.Image == productDto.Image)), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Id = 1,
                Name = "Updated Product",
                Description = "Updated description",
                Price = 30.5m,
                CategoryId = 2,
                Image = "updated_image.png"
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Domain.Entities.Product)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _updateProduct.ExecuteAsync(productDto));
            Assert.Equal("Produto não encontrado.", exception.Message);
        }
    }
}
