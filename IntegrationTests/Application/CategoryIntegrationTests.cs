using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using FluentAssertions;

namespace IntegrationTests.Application
{
    public class CategoryIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public CategoryIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RegisterCategory_ShouldReturnCreated_WhenValidCategory()
        {
            // Arrange
            var category = new
            {
                Name = "Bebidas",
                Description = "Todas as bebidas"
            };

            var content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/categories", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Get_Category_By_Id_Should_Return_Category()
        {
            // Arrange
            var categoryId = 1; // Assume this ID exists in the test database

            // Act
            var response = await _client.GetAsync($"/api/categories/{categoryId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Hamburgers", responseString);
        }

        [Fact]
        public async Task Update_Category_Should_Return_NoContent()
        {
            // Arrange
            var category = new { Id = 1, Name = "Updated Hamburgers", Description = "Updated description" };
            var content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/categories/{category.Id}", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Get_All_Categories_Should_Return_List()
        {
            // Act
            var response = await _client.GetAsync("/api/categories");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseString);
        }
    }
}
