using Application.DTOs;
using Application.Interfaces;
using Application.UseCases.Products;
using Domain.Entities;
using Moq;

namespace UnitTests.UseCases.Products
{
    public class CreateProductTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly CreateProduct _createProduct;

        public CreateProductTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _createProduct = new CreateProduct(_mockProductRepository.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldCallAddAsync_WhenProductDtoIsValid()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "Cheeseburger",
                Description = "Hambúrguer com queijo",
                Price = 10,
                CategoryId = 1
            };

            // Act
            await _createProduct.ExecuteAsync(productDto);

            // Assert
            // Verifica se o método AddAsync foi chamado uma vez com o produto correto
            _mockProductRepository.Verify(repo => repo.AddAsync(It.Is<Product>(p =>
                p.Name == productDto.Name &&
                p.Description == productDto.Description &&
                p.Price == productDto.Price &&
                p.CategoryId == productDto.CategoryId &&
                p.Image == productDto.Image)), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldNotCallAddAsync_WhenProductDtoIsNull()
        {
            // Arrange
            ProductDto? productDto = null;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _createProduct.ExecuteAsync(productDto));

            Assert.Equal("Produto não pode ser nulo (Parameter 'productDto')", exception.Message);
        }
    }
}
