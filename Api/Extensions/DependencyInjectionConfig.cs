using Application.Interfaces;
using Application.UseCases.Categories;
using Application.UseCases.Products;
using Infrastructure.Configurations.Database;
using Infrastructure.Data.Initializer;
using Infrastructure.Repositories;

namespace Api.Extensions
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddResolveDependencies(this WebApplicationBuilder builder)
        {
            return AddResolveDependencies(builder.Services, builder.Configuration);
        }

        public static IServiceCollection AddResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDbConnectionFactory, PostgreSqlConnectionFactory>();

            // Inicializador do banco de dados
            services.AddSingleton<DatabaseInitializer>();

            //Category
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<RegisterCategory>();
            services.AddScoped<UpdateCategory>();
            services.AddScoped<GetCategoryById>();
            services.AddScoped<GetCategoryByName>();
            services.AddScoped<GetAllCategories>();

            //Product
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<CreateProduct>();
            services.AddScoped<UpdateProduct>();
            services.AddScoped<GetProductById>();
            services.AddScoped<GetProductsByCategoryId>();
            services.AddScoped<GetAllProducts>();
            services.AddScoped<DeleteProduct>();

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddHttpClient();

            return services;
        }
    }
}
