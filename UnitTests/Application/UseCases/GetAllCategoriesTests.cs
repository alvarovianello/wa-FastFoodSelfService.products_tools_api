using Application.Interfaces;
using Application.UseCases.Categories;
using Domain.Entities;
using Moq;

namespace UnitTests.Application.UseCases
{
    public class GetAllCategoriesTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly GetAllCategories _getAllCategoriesUseCase;

        public GetAllCategoriesTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _getAllCategoriesUseCase = new GetAllCategories(_categoryRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_Return_All_Categories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Hamburgers", Description = "Delicious hamburgers" },
                new Category { Id = 2, Name = "Fries", Description = "Crispy fries" }
            };

            _categoryRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(categories);

            // Act
            var result = await _getAllCategoriesUseCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.ToList().Count);
        }
    }
}
