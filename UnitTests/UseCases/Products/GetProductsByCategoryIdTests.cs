using Application.Interfaces;
using Application.UseCases.Products;
using Moq;

namespace UnitTests.UseCases.Products
{
    public class GetProductsByCategoryIdTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly GetProductsByCategoryId _getProductsByCategoryId;

        public GetProductsByCategoryIdTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _getProductsByCategoryId = new GetProductsByCategoryId(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnProductDtos_WhenProductsExistForCategory()
        {
            // Arrange
            var products = new List<Domain.Entities.Product>
            {
                new Domain.Entities.Product
                {
                    Id = 1,
                    Name = "Product 1",
                    Description = "Product 1 description",
                    Price = 10.5m,
                    CategoryId = 1,
                    Image = "image1.png"
                },
                new Domain.Entities.Product
                {
                    Id = 2,
                    Name = "Product 2",
                    Description = "Product 2 description",
                    Price = 20.5m,
                    CategoryId = 1,
                    Image = "image2.png"
                }
            };

            _productRepositoryMock.Setup(repo => repo.GetByCategoryIdAsync(1)).ReturnsAsync(products);

            // Act
            var result = await _getProductsByCategoryId.ExecuteAsync(1);

            // Assert
            Assert.NotNull(result);
            var productDtos = result.ToList();
            Assert.Equal(2, productDtos.Count);
            Assert.All(productDtos, productDto =>
            {
                Assert.NotNull(productDto);
                Assert.Equal(1, productDto.CategoryId);
            });
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnEmptyList_WhenNoProductsExistForCategory()
        {
            // Arrange
            _productRepositoryMock.Setup(repo => repo.GetByCategoryIdAsync(99)).ReturnsAsync(new List<Domain.Entities.Product>());

            // Act
            var result = await _getProductsByCategoryId.ExecuteAsync(99);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
