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

            var existingCategory = await _categoryRepository.GetByIdAsync(categoryDto.Id);
            if (existingCategory == null)
            {
                throw new InvalidOperationException("Categoria não encontrada.");
            }

            // Verifica se o nome já existe, excluindo a categoria atual
            if (await _categoryRepository.ExistsByNameAsync(categoryDto.Name, categoryDto.Id))
            {
                throw new InvalidOperationException("O nome de categoria informado já possui cadastro.");
            }

            // Atualiza os dados da categoria
            existingCategory.Name = categoryDto.Name;
            existingCategory.Description = categoryDto.Description;

            await _categoryRepository.UpdateAsync(existingCategory);
        }
    }
}
