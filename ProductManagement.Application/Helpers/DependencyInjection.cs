using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.IServices;
using ProductManagement.Application.RepoContracts.IWishListRepos;
using ProductManagement.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Helpers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind strongly-typed settings with validation on startup
            services.AddOptions<PaginationSetup>()
                    .Bind(configuration.GetSection("PaginationSetup"))  
                    .ValidateOnStart();

            services.AddOptions<PriceConstsSetup>()
                    .Bind(configuration.GetSection("PriceConstsSetup"))
                    .ValidateOnStart();

            // Register MediatR
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(MediatorAssembly).Assembly));

            // Register services
            services.AddScoped<IRegionService, RegionService>();

            services.AddScoped<IWishListService, WishListService>();

            services.AddScoped<ICartService, CartService>();



            services.AddHttpClient<UserMicroClient>(client =>
            {
                client.BaseAddress = 
                new Uri($"https://{configuration["UserMicroHostName"]}:{configuration["UserMicroPortNumber"]}");


            });

            return services;
        }
    }
}
