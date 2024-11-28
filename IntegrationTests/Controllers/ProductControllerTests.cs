using Application.DTOs;
using Newtonsoft.Json;
using System.Text;

namespace IntegrationTests.Controllers
{
    public class ProductControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(); 
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnOk_WhenProductExists()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Id = 1,
                Name = "Cheeseburger",
                Description = "Hambúrguer com queijo",
                CategoryId = 1,
                Price = 10
            };

            var content = new StringContent(JsonConvert.SerializeObject(productDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/product/{productDto.Id}", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnBadRequest_WhenProductIdMismatch()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Id = 1,
                Name = "Cheeseburger",
                Description = "Hambúrguer com queijo",
                CategoryId = 1,
                Price = 10
            };

            var content = new StringContent(JsonConvert.SerializeObject(productDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/product/99", content); 

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
       

        [Fact]
        public async Task DeleteProduct_ShouldReturnBadRequest_WhenProductDoesNotExist()
        {
            // Act
            var response = await _client.DeleteAsync("/api/product/999");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnOk_WhenProductExists()
        {
            // Act
            var response = await _client.GetAsync("/api/product/1");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Act
            var response = await _client.GetAsync("/api/product/999");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetProductsByCategoryId_ShouldReturnOk_WhenProductsExist()
        {
            // Act
            var response = await _client.GetAsync("/api/product/category/1");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetProductsByCategoryId_ShouldReturnEmpty_WhenNoProductsExistForCategory()
        {
            // Act
            var response = await _client.GetAsync("/api/product/category/999");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnOk_WhenProductsExist()
        {
            // Act
            var response = await _client.GetAsync("/api/product/all");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
    }
}
