using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases.Categories
{
    public class RegisterCategory
    {
        private readonly ICategoryRepository _categoryRepository;

        public RegisterCategory(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task ExecuteAsync(CategoryDto categoryDto)
        {

            if (string.IsNullOrWhiteSpace(categoryDto.Name))
            {
                throw new InvalidOperationException("O nome da categoria não pode ser vazio.");
            }

            // Verifica se o nome já existe
            if (await _categoryRepository.ExistsByNameAsync(categoryDto.Name))
            {
                throw new InvalidOperationException("O nome de categoria informado já possui cadastro.");
            }

            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };
            await _categoryRepository.AddAsync(category);
        }
    }
}
