using Application.DTOs;
using Application.Interfaces;
using Application.UseCases.Categories;
using Domain.Entities;
using Moq;
using TechTalk.SpecFlow;

namespace UnitTests.Steps.Categories
{
    [Binding]
    public class GetAllCategoriesSteps
    {
        private readonly Mock<ICategoryRepository> _mockRepository;
        private readonly GetAllCategories _useCase;
        private List<Category> _categoryList;
        private IEnumerable<CategoryDto> _result;

        public GetAllCategoriesSteps()
        {
            _mockRepository = new Mock<ICategoryRepository>();
            _useCase = new GetAllCategories(_mockRepository.Object);
        }

        [Given(@"que a API tem as seguintes categorias")]
        public void GivenThatTheAPIHasTheFollowingCategories(Table table)
        {
            _categoryList = table.Rows.Select(row => new Category
            {
                Id = int.Parse(row["Id"]),
                Name = row["Name"],
                Description = row["Description"]
            }).ToList();

            _mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(_categoryList);
        }

        [When(@"solicito todas as categorias")]
        public async Task WhenIRequestAllCategories()
        {
            _result = await _useCase.ExecuteAsync();
        }

        [Then(@"a resposta deve conter (.*) categorias")]
        public void ThenTheResponseShouldContainCategories(int count)
        {
            Assert.NotNull(_result);
            Assert.Equal(count, _result.Count());
        }

        [Then(@"a resposta devera incluir uma categoria com o nome ""(.*)""")]
        public void ThenTheResponseShouldIncludeACategoryWithName(string name)
        {
            Assert.Contains(_result, category => category.Name == name);
        }
    }
}
