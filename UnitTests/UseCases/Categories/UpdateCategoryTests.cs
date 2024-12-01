using Application.DTOs;
using Application.Interfaces;
using Application.UseCases.Categories;
using Domain.Entities;
using Moq;

namespace UnitTests.UseCases.Categories
{
    public class UpdateCategoryTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly UpdateCategory _updateCategory;

        public UpdateCategoryTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _updateCategory = new UpdateCategory(_categoryRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldUpdateCategory_WhenCategoryExists()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Id = 1,
                Name = "Updated Hamburgers",
                Description = "Updated description"
            };

            var existingCategory = new Category
            {
                Id = 1,
                Name = "Hamburgers",
                Description = "Original description"
            };

            _categoryRepositoryMock
                .Setup(repo => repo.GetByIdAsync(categoryDto.Id))
                .ReturnsAsync(existingCategory);

            _categoryRepositoryMock
                .Setup(repo => repo.ExistsByNameAsync(categoryDto.Name, categoryDto.Id))
                .ReturnsAsync(false);

            // Act
            await _updateCategory.ExecuteAsync(categoryDto);

            // Assert
            _categoryRepositoryMock.Verify(repo => repo.GetByIdAsync(categoryDto.Id), Times.Once);
            _categoryRepositoryMock.Verify(repo => repo.ExistsByNameAsync(categoryDto.Name, categoryDto.Id), Times.Once);
            _categoryRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Category>(c =>
                c.Id == categoryDto.Id &&
                c.Name == categoryDto.Name &&
                c.Description == categoryDto.Description
            )), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowException_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Id = 99, // ID inexistente
                Name = "Nonexistent Category",
                Description = "Nonexistent description"
            };

            _categoryRepositoryMock
                .Setup(repo => repo.GetByIdAsync(categoryDto.Id))
                .ReturnsAsync((Category?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _updateCategory.ExecuteAsync(categoryDto));
            Assert.Equal("Categoria não encontrada.", exception.Message);

            _categoryRepositoryMock.Verify(repo => repo.GetByIdAsync(categoryDto.Id), Times.Once);
            _categoryRepositoryMock.Verify(repo => repo.ExistsByNameAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
            _categoryRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowException_WhenCategoryNameAlreadyExists()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Id = 1,
                Name = "Duplicate Name",
                Description = "Updated description"
            };

            var existingCategory = new Category
            {
                Id = 1,
                Name = "Hamburgers",
                Description = "Original description"
            };

            _categoryRepositoryMock
                .Setup(repo => repo.GetByIdAsync(categoryDto.Id))
                .ReturnsAsync(existingCategory);

            _categoryRepositoryMock
                .Setup(repo => repo.ExistsByNameAsync(categoryDto.Name, categoryDto.Id))
                .ReturnsAsync(true); 

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _updateCategory.ExecuteAsync(categoryDto));
            Assert.Equal("O nome de categoria informado já possui cadastro.", exception.Message);

            _categoryRepositoryMock.Verify(repo => repo.GetByIdAsync(categoryDto.Id), Times.Once);
            _categoryRepositoryMock.Verify(repo => repo.ExistsByNameAsync(categoryDto.Name, categoryDto.Id), Times.Once);
            _categoryRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }

    }
}
