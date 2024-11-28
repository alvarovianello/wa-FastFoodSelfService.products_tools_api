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
            // Verifica se o nome já existe
            if (await _categoryRepository.ExistsByNameAsync(categoryDto.Name))
            {
                throw new Exception("O nome de categoria informado já possui cadastro.");
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
