using Application.DTOs;
using Newtonsoft.Json;
using System.Text;

namespace IntegrationTests.Controllers
{
    public class CategoryControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public CategoryControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();  // Cria um cliente HTTP para fazer as requisições
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnOk_WhenCategoryExists()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Id = 1,
                Name = "Lanche",
                Description = "Sanduíches e hambúrgueres variados"
            };

            var content = new StringContent(JsonConvert.SerializeObject(categoryDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/category/{categoryDto.Id}", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnBadRequest_WhenCategoryIdMismatch()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Id = 1,
                Name = "Lanche",
                Description = "Sanduíches e hambúrgueres variados"
            };

            var content = new StringContent(JsonConvert.SerializeObject(categoryDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/category/999", content); // ID diferente

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnOk_WhenCategoryExists()
        {
            // Act
            var response = await _client.GetAsync("/api/category/1");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            // Act
            var response = await _client.GetAsync("/api/category/999");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetCategoryByName_ShouldReturnOk_WhenCategoryExists()
        {
            // Act
            var response = await _client.GetAsync("/api/category/name/Lanche");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetCategoryByName_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            // Act
            var response = await _client.GetAsync("/api/category/name/NonExistent");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetAllCategories_ShouldReturnOk_WhenCategoriesExist()
        {
            // Act
            var response = await _client.GetAsync("/api/category/all");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
    }
}
