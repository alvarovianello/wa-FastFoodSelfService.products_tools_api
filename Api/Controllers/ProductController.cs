using Application.DTOs;
using Application.UseCases.Products;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly GetAllProducts _getAllProducts;
        private readonly GetProductById _getProductById;
        private readonly GetProductsByCategoryId _getProductsByCategoryId;
        private readonly CreateProduct _createProduct;
        private readonly UpdateProduct _updateProduct;
        private readonly DeleteProduct _deleteProduct;

        public ProductController(
            GetAllProducts getAllProducts,
            GetProductById getProductById,
            GetProductsByCategoryId getProductsByCategoryId,
            CreateProduct createProduct,
            UpdateProduct updateProduct,
            DeleteProduct deleteProduct)
        {
            _getAllProducts = getAllProducts;
            _getProductById = getProductById;
            _getProductsByCategoryId = getProductsByCategoryId;
            _createProduct = createProduct;
            _updateProduct = updateProduct;
            _deleteProduct = deleteProduct;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts([FromQuery] string productIds = null)
        {
            var products = await _getAllProducts.ExecuteAsync(productIds);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _getProductById.ExecuteAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategoryId(int categoryId)
        {
            var products = await _getProductsByCategoryId.ExecuteAsync(categoryId);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            await _createProduct.ExecuteAsync(productDto);
            return CreatedAtAction(nameof(GetProductById), new { id = productDto.Id }, productDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            if (id != productDto.Id) return BadRequest("ID do produto não corresponde.");

            try
            {
                await _updateProduct.ExecuteAsync(productDto);
                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {            
            try
            {
                await _deleteProduct.ExecuteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
