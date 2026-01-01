
using Microsoft.OpenApi.Models;
using ProductManagement.Presentation;
using System;
using System.Collections.Generic;

namespace ProductManagement.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // Add controllers to the service.
            services.AddControllers()
                .AddApplicationPart(typeof(PresentationalAssembly).Assembly);

            services.AddEndpointsApiExplorer();
            services.AddOpenApi(); // Simple registration
            return services;
        }
    }
}
