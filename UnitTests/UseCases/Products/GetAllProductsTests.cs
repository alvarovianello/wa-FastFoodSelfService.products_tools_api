using Application.Interfaces;
using Application.UseCases.Products;
using Domain.Entities;
using Moq;

namespace UnitTests.UseCases.Products
{
    public class GetAllProductsTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly GetAllProducts _getAllProducts;

        public GetAllProductsTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _getAllProducts = new GetAllProducts(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnAllProducts_WhenProductIdsIsNullOrEmpty()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10, CategoryId = 1 },
                new Product { Id = 2, Name = "Product 2", Price = 20, CategoryId = 1 }
            };

            _productRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _getAllProducts.ExecuteAsync("");

            // Assert
            var resultList = result.ToList();
            Assert.Equal(2, resultList.Count);
            Assert.Equal("Product 1", resultList[0].Name);
            Assert.Equal(10, resultList[0].Price);
            Assert.Equal("Product 2", resultList[1].Name);
            Assert.Equal(20, resultList[1].Price);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFilteredProducts_WhenProductIdsAreProvided()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10, CategoryId = 1 },
                new Product { Id = 2, Name = "Product 2", Price = 20, CategoryId = 1 },
                new Product { Id = 3, Name = "Product 3", Price = 30, CategoryId = 2 }
            };

            _productRepositoryMock.Setup(repo => repo.GetByFilterAsync(It.IsAny<int[]>()))
                .ReturnsAsync(products.Where(p => p.Id == 1 || p.Id == 3).ToList());

            // Act
            var result = await _getAllProducts.ExecuteAsync("1,3");

            // Assert
            var resultList = result.ToList();
            Assert.Equal(2, resultList.Count);
            Assert.Contains(resultList, p => p.Id == 1);
            Assert.Contains(resultList, p => p.Id == 3);
            Assert.DoesNotContain(resultList, p => p.Id == 2);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnEmpty_WhenNoProductIdsMatch()
        {
            // Arrange
            _productRepositoryMock.Setup(repo => repo.GetByFilterAsync(It.IsAny<int[]>()))
                .ReturnsAsync(new List<Product>());

            // Act
            var result = await _getAllProducts.ExecuteAsync("99");

            // Assert
            var resultList = result.ToList();
            Assert.Empty(resultList);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldIgnoreInvalidIds_WhenParsingFails()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10, CategoryId = 1 },
                new Product { Id = 2, Name = "Product 2", Price = 20, CategoryId = 1 }
            };

            _productRepositoryMock.Setup(repo => repo.GetByFilterAsync(It.IsAny<int[]>()))
                .ReturnsAsync(products.Where(p => p.Id == 1).ToList());

            // Act
            var result = await _getAllProducts.ExecuteAsync("1,invalid,2.5,!@#");

            // Assert
            var resultList = result.ToList();
            Assert.Single(resultList);
            Assert.Contains(resultList, p => p.Id == 1);
            _productRepositoryMock.Verify(repo => repo.GetByFilterAsync(new[] { 1 }), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldHandleEmptyValidIdsAfterFiltering()
        {
            // Arrange
            _productRepositoryMock.Setup(repo => repo.GetByFilterAsync(It.IsAny<int[]>()))
                .ReturnsAsync(new List<Product>());

            // Act
            var result = await _getAllProducts.ExecuteAsync("invalid,!@#,abc");

            // Assert
            var resultList = result.ToList();
            Assert.Empty(resultList);
            _productRepositoryMock.Verify(repo => repo.GetByFilterAsync(It.IsAny<int[]>()), Times.Once);
        }
    }
}
