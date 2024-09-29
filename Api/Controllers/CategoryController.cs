using Application.DTOs;
using Application.UseCases.Categories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly RegisterCategory _registerCategory;
        private readonly UpdateCategory _updateCategory;
        private readonly GetCategoryById _getCategoryById;
        private readonly GetCategoryByName _getCategoryByName;
        private readonly GetAllCategories _getAllCategories;

        public CategoryController(
            RegisterCategory registerCategory,
            UpdateCategory updateCategory,
            GetCategoryById getCategoryById,
            GetCategoryByName getCategoryByName,
            GetAllCategories getAllCategories)
        {
            _registerCategory = registerCategory;
            _updateCategory = updateCategory;
            _getCategoryById = getCategoryById;
            _getCategoryByName = getCategoryByName;
            _getAllCategories = getAllCategories;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCategory([FromBody] CategoryDto categoryDto)
        {
            try
            {
                await _registerCategory.ExecuteAsync(categoryDto);
                return CreatedAtAction(nameof(GetCategoryById), new { id = categoryDto.Id }, categoryDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {

            if (id != categoryDto.Id) return BadRequest("ID da categoria não corresponde.");

            try
            {
                await _updateCategory.ExecuteAsync(categoryDto);
                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _getCategoryById.ExecuteAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            var category = await _getCategoryByName.ExecuteAsync(name);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _getAllCategories.ExecuteAsync();
            return Ok(categories);
        }
    }
}
