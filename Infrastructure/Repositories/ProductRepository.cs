using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Dapper;
using Npgsql;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;

        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task AddAsync(Product product)
        {
            var query = "INSERT INTO dbo.Product (category_id, name, description, price, image) VALUES (@CategoryId, @Name, @Description, @Price, @Image)";
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(query, product);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var query = "DELETE FROM dbo.Product WHERE id = @Id";
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var query = "SELECT *, category_id as CategoryId FROM dbo.Product";
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<Product>(query);
            }
        }

        public async Task<IEnumerable<Product>> GetByFilterAsync(int[] productIds)
        {
            var query = "SELECT *, category_id as CategoryId FROM dbo.Product WHERE id = ANY(@Ids)";
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<Product>(query, new { Ids = productIds });
            }
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var query = "SELECT *, category_id as CategoryId FROM dbo.Product WHERE id = @Id";
            using (var connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Product>(query, new { Id = id });
            }
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            var query = "SELECT *, category_id as CategoryId FROM dbo.Product WHERE category_id = @CategoryId";
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<Product>(query, new { CategoryId = categoryId });
            }
        }

        public async Task UpdateAsync(Product product)
        {
            var query = "UPDATE dbo.Product SET category_id = @CategoryId, name = @Name, description = @Description, price = @Price, image = @Image WHERE id = @Id";
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(query, product);
            }
        }
    }
}