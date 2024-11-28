using Application.DTOs;
using Application.Interfaces;
using Application.UseCases.Categories;
using Domain.Entities;
using Moq;

namespace UnitTests.Application.UseCases
{
    public class UpdateCategoryTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly UpdateCategory _updateCategoryUseCase;

        public UpdateCategoryTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _updateCategoryUseCase = new UpdateCategory(_categoryRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_Update_Category_Successfully()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Id = 1,
                Name = "Hamburgers",
                Description = "Updated description"
            };

            // Act
            await _updateCategoryUseCase.ExecuteAsync(categoryDto);

            // Assert
            _categoryRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_Exception_When_Category_Not_Found()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Id = 99, // ID that does not exist
                Name = "Hamburgers",
                Description = "Description"
            };

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Category)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _updateCategoryUseCase.ExecuteAsync(categoryDto));
        }
    }
}
