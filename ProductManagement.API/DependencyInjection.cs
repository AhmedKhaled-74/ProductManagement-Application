
using ProductManagement.Presentation;
using System;
using System.Collections.Generic;

namespace ProductManagement.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {

            // Add controllers to the service.
            services.AddControllers()
                .AddApplicationPart(typeof(PresentationalAssembly).Assembly);

            services.AddOpenApi();
            return services;
        }
    }
}
