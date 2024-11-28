using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases.Categories
{
    public class UpdateCategory
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategory(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task ExecuteAsync(CategoryDto categoryDto)
        {
            // Verifica se o nome já existe, excluindo a categoria atual
            if (await _categoryRepository.ExistsByNameAsync(categoryDto.Name, categoryDto.Id))
            {
                throw new Exception("O nome de categoria informado já possui cadastro.");
            }

            var category = new Category
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };
            await _categoryRepository.UpdateAsync(category);
        }
    }
}
