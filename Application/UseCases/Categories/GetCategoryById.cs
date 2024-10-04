using Application.DTOs;
using Application.Interfaces;

namespace Application.UseCases.Categories
{
    public class GetCategoryById
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryById(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> ExecuteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
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
