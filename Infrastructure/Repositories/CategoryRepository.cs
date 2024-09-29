using Application.Interfaces;
using Dapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IConfiguration _configuration;

        public CategoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task AddAsync(Category category)
        {
            var query = "INSERT INTO dbo.Category (name, description) VALUES (@Name, @Description)";
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(query, new { category.Name, category.Description });
            }
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var query = "SELECT * FROM dbo.Category";
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<Category>(query);
            }
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM dbo.Category WHERE id = @Id";
            using (var connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Category>(query, new { Id = id });
            }
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            var query = "SELECT * FROM dbo.Category WHERE name = @Name";
            using (var connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Category>(query, new { Name = name });
            }
        }

        public async Task UpdateAsync(Category category)
        {
            var query = "UPDATE dbo.Category SET name = @Name, description = @Description WHERE id = @Id";
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(query, new { category.Name, category.Description, category.Id });
            }
        }

        public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        {
            var query = "SELECT COUNT(*) FROM dbo.Category WHERE name = @Name" +
                        (excludeId.HasValue ? " AND id != @ExcludeId" : string.Empty);

            using (var connection = CreateConnection())
            {
                var count = await connection.ExecuteScalarAsync<int>(query, new { Name = name, ExcludeId = excludeId });
                return count > 0;
            }
        }
    }
}
