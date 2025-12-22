using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Application.IServices;
using ProductManagement.Application.RepoContracts;
using ProductManagement.Application.RepoContracts.ICartRepo;
using ProductManagement.Application.RepoContracts.IProductRepos.Common;
using ProductManagement.Application.RepoContracts.IWishListRepos;
using ProductManagement.Application.Services;
using ProductManagement.Infrastructure.DbContexts;
using ProductManagement.Infrastructure.Repos.CartRepos;
using ProductManagement.Infrastructure.Repos.ProductRepos.Common;
using ProductManagement.Infrastructure.Repos.WishListRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Repos
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default")));

            services.AddDbContext<AppReadDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); // Important for reads
            });

            services.AddScoped<IRegionRepo, RegionRepo>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<IProductGetterRepo, ProductGettersRepo>();
            services.AddScoped<IProductSettersRepo, ProductSettersRepo>();
            services.AddScoped<IWishListGettersRepo, WishListGettersRepo>();
            services.AddScoped<IWishListSettersRepo, WishListSettersRepo>();
            services.AddScoped<ICartGettersRepo, CartGettersRepo>();
            services.AddScoped<ICartSettersRepo, CartSettersRepo>();

            return services;
        }
    }
}
