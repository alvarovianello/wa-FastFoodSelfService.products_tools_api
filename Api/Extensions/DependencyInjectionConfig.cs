using Application.Interfaces;
using Application.UseCases.Categories;
using Application.UseCases.Products;
using Infrastructure.Repositories;

namespace Api.Extensions
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddResolveDependencies(this WebApplicationBuilder builder)
        {
            IServiceCollection services = builder.Services;

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
