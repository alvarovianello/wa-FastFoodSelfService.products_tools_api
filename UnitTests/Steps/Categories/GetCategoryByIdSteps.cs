using Application.DTOs;
using Application.Interfaces;
using Application.UseCases.Categories;
using Domain.Entities;
using Moq;
using TechTalk.SpecFlow;

namespace UnitTests.Steps.Categories
{
    [Binding]
    public class GetCategoryByIdSteps
    {
        private readonly Mock<ICategoryRepository> _mockRepository;
        private readonly GetCategoryById _useCase;
        private CategoryDto _result;

        public GetCategoryByIdSteps()
        {
            _mockRepository = new Mock<ICategoryRepository>();
            _useCase = new GetCategoryById(_mockRepository.Object);
        }

        [Given(@"que a API tem a categoria com Id (.*)")]
        public void GivenThatTheAPIHasACategoryWithId(int id, Table table)
        {
            var categoryData = table.Rows.Single();

            var category = new Category
            {
                Id = id,
                Name = categoryData["Name"],
                Description = categoryData["Description"]
            };

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(id))
                .ReturnsAsync(category);
        }

        [When(@"solicito a categoria pelo Id (.*)")]
        public async Task WhenISolicitCategoryById(int id)
        {
            _result = await _useCase.ExecuteAsync(id);
        }

        [Then(@"a resposta deve conter a categoria com Id (.*)")]
        public void ThenTheResponseShouldContainTheCategoryWithId(int id)
        {
            Assert.NotNull(_result);
            Assert.Equal(id, _result.Id);
        }

        [Then(@"a resposta devera incluir o nome ""(.*)""")]
        public void ThenTheResponseShouldIncludeTheName(string name)
        {
            Assert.Equal(name, _result.Name);
        }
    }
}
