using Application.DTOs;
using Application.Interfaces;
using Application.UseCases.Products;
using Domain.Entities;
using Moq;
using TechTalk.SpecFlow;

namespace UnitTests.Steps.Products
{
    [Binding]
    public class GetAllProductsSteps
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly GetAllProducts _useCase;
        private IEnumerable<Product> _productList;
        private IEnumerable<ProductDto> _result;

        public GetAllProductsSteps()
        {
            _mockRepository = new Mock<IProductRepository>();
            _useCase = new GetAllProducts(_mockRepository.Object);
        }

        [Given(@"que a API tem os seguintes produtos")]
        public void GivenTheAPIHasTheFollowingProducts(Table table)
        {
            _productList = table.Rows.Select(row => new Product
            {
                Id = int.Parse(row["Id"]),
                CategoryId = int.Parse(row["CategoryId"]),
                Name = row["Name"],
                Description = row["Description"],
                Price = decimal.Parse(row["Price"]),
                Image = row["Image"]
            });

            _mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(_productList);

            _mockRepository
                .Setup(repo => repo.GetByFilterAsync((int[])It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync((IEnumerable<int> ids) =>
                    _productList.Where(p => ids.Contains(p.Id)));
        }

        [When(@"solicito todos os produtos")]
        public async Task WhenIRequestAllProducts()
        {
            _result = await _useCase.ExecuteAsync(null);
        }

        [When(@"solicito produtos com os IDs ""(.*)""")]
        public async Task WhenIRequestProductsWithIds(string productIds)
        {
            _result = await _useCase.ExecuteAsync(productIds);
        }

        [Then(@"a resposta deve conter (.*) produtos")]
        public void ThenTheResponseShouldContainProducts(int count)
        {
            Assert.NotNull(_result);
            Assert.Equal(count, _result.Count());
        }

        [Then(@"a resposta deve incluir um produto com o nome ""(.*)""")]
        public void ThenTheResponseShouldIncludeAProductWithName(string name)
        {
            Assert.Contains(_result, product => product.Name == name);
        }
    }
}
