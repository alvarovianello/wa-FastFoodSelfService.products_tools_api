using Application.DTOs;
using Application.Interfaces;

namespace Application.UseCases.Categories
{
    public class GetCategoryByName
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByName(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> ExecuteAsync(string name)
        {
            var category = await _categoryRepository.GetByNameAsync(name);
            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }
    }
}
