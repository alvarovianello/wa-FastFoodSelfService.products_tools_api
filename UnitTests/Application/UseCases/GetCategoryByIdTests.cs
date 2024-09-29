using Application.Interfaces;
using Application.UseCases.Categories;
using Domain.Entities;
using Moq;

namespace UnitTests.Application.UseCases
{
    public class GetCategoryByIdTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly GetCategoryById _getCategoryByIdUseCase;

        public GetCategoryByIdTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _getCategoryByIdUseCase = new GetCategoryById(_categoryRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_Return_Category_When_Found()
        {
            // Arrange
            var category = new Category
            {
                Id = 1,
                Name = "Hamburgers",
                Description = "Delicious hamburgers"
            };

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(category);

            // Act
            var result = await _getCategoryByIdUseCase.ExecuteAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task Should_Return_Null_When_Category_Not_Found()
        {
            // Arrange
            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Category)null);

            // Act
            var result = await _getCategoryByIdUseCase.ExecuteAsync(1);

            // Assert
            Assert.Null(result);
        }
    }
}
