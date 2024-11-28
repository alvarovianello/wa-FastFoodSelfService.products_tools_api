using Application.DTOs;
using Application.Interfaces;
using Application.UseCases.Categories;
using Domain.Entities;
using Moq;

namespace UnitTests.UseCases.Categories
{
    public class RegisterCategoryTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly RegisterCategory _registerCategoryUseCase;

        public RegisterCategoryTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _registerCategoryUseCase = new RegisterCategory(_categoryRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_Register_Category_Successfully()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Name = "Hamburgers",
                Description = "Delicious hamburgers"
            };

            // Act
            await _registerCategoryUseCase.ExecuteAsync(categoryDto);

            // Assert
            _categoryRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_Exception_When_Name_Is_Empty()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Name = "",
                Description = "Description"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _registerCategoryUseCase.ExecuteAsync(categoryDto));
        }

        [Fact]
        public async Task Should_Throw_Exception_When_Name_Already_Exists()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Name = "Hamburgers",
                Description = "Delicious hamburgers"
            };

            _categoryRepositoryMock
                .Setup(repo => repo.ExistsByNameAsync(categoryDto.Name,null))
                .ReturnsAsync(true); // Simula que o nome já existe

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _registerCategoryUseCase.ExecuteAsync(categoryDto));

            Assert.Equal("O nome de categoria informado já possui cadastro.", exception.Message);

            // Verifica que AddAsync nunca é chamado quando o nome já existe
            _categoryRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Category>()), Times.Never);
        }
    }
}
