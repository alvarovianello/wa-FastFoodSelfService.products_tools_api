using Application.Interfaces;
using Application.UseCases.Categories;
using Domain.Entities;
using Moq;

namespace UnitTests.Application.UseCases
{
    public class GetCategoryByNameTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly GetCategoryByName _getCategoryByNameUseCase;

        public GetCategoryByNameTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _getCategoryByNameUseCase = new GetCategoryByName(_categoryRepositoryMock.Object);
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

            _categoryRepositoryMock.Setup(x => x.GetByNameAsync("Hamburgers")).ReturnsAsync(category);

            // Act
            var result = await _getCategoryByNameUseCase.ExecuteAsync("Hamburgers");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task Should_Return_Null_When_Category_Not_Found()
        {
            // Arrange
            _categoryRepositoryMock.Setup(x => x.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((Category)null);

            // Act
            var result = await _getCategoryByNameUseCase.ExecuteAsync("NonExistent");

            // Assert
            Assert.Null(result);
        }
    }
}
