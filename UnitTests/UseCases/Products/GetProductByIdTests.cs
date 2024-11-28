using Application.Interfaces;
using Application.UseCases.Products;
using Moq;

namespace UnitTests.UseCases.Products
{
    public class GetProductByIdTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly GetProductById _getProductById;

        public GetProductByIdTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _getProductById = new GetProductById(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnProductDto_WhenProductExists()
        {
            // Arrange
            var product = new Domain.Entities.Product
            {
                Id = 1,
                Name = "Product 1",
                Description = "Product 1 description",
                Price = 10.5m,
                CategoryId = 1,
                Image = "image.png"
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _getProductById.ExecuteAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Product 1", result.Name);
            Assert.Equal("Product 1 description", result.Description);
            Assert.Equal(10.5m, result.Price);
            Assert.Equal(1, result.CategoryId);
            Assert.Equal("image.png", result.Image);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Domain.Entities.Product)null);

            // Act
            var result = await _getProductById.ExecuteAsync(999);

            // Assert
            Assert.Null(result);
        }
    }
}
