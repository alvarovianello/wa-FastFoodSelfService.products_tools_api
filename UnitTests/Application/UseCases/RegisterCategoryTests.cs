using Application.DTOs;
using Application.Interfaces;
using Application.UseCases.Categories;
using Domain.Entities;
using Moq;

namespace UnitTests.Application.UseCases
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
            await Assert.ThrowsAsync<ArgumentException>(() => _registerCategoryUseCase.ExecuteAsync(categoryDto));
        }
    }
}
