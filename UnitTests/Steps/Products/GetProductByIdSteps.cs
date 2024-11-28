using Application.DTOs;
using Application.Interfaces;
using Application.UseCases.Products;
using Domain.Entities;
using Moq;
using TechTalk.SpecFlow;

namespace UnitTests.Steps.Products
{
    [Binding]
    public class GetProductByIdSteps
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly GetProductById _useCase;
        private Product _product;
        private ProductDto _result;

        public GetProductByIdSteps()
        {
            _mockRepository = new Mock<IProductRepository>();
            _useCase = new GetProductById(_mockRepository.Object);
        }

        [Given(@"que a API tem o produto com Id (.*)")]
        public void GivenTheAPIHasAProductWithId(int id, Table table)
        {
            var row = table.Rows.First();
            _product = new Product
            {
                Id = int.Parse(row["Id"]),
                CategoryId = int.Parse(row["CategoryId"]),
                Name = row["Name"],
                Description = row["Description"],
                Price = decimal.Parse(row["Price"]),
                Image = row["Image"]
            };

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(_product);
        }

        [Given(@"que a API nao tem nenhum produto com Id (.*)")]
        public void GivenTheAPIHasNoProductWithId(int id)
        {
            _mockRepository
                .Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync((Product)null);
        }

        [When(@"solicito o produto pelo Id (.*)")]
        public async Task WhenIRequestProductById(int id)
        {
            _result = await _useCase.ExecuteAsync(id);
        }

        [Then(@"a resposta deve conter o produto com Id (.*)")]
        public void ThenTheResponseShouldContainTheProductWithId(int id)
        {
            Assert.NotNull(_result);
            Assert.Equal(id, _result.Id);
        }

        [Then(@"a resposta deve incluir o nome ""(.*)""")]
        public void ThenTheResponseShouldIncludeTheName(string name)
        {
            Assert.Equal(name, _result.Name);
        }

        [Then(@"a resposta deve ser nula")]
        public void ThenTheResponseShouldBeNull()
        {
            Assert.Null(_result);
        }
    }
}
