using Application.DTOs;
using Application.Interfaces;

namespace Application.UseCases.Categories
{
    public class GetAllCategories
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategories(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> ExecuteAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return categories.Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            });
        }
    }
}
