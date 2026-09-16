using BlogService.Application.Interfaces.Repositories;
using BlogService.Infrastructure.presistence;
using BlogService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<BlogDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultString")));

        services.AddScoped<IBlogRepository, BlogRepository>();

        return services;
    }
}