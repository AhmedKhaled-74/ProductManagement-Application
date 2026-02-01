using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.Helpers.Policies;
using ProductManagement.Application.HttpClients;
using ProductManagement.Application.IServices;
using ProductManagement.Application.Services;
using StackExchange.Redis;

namespace ProductManagement.Application.Helpers
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services,
            IConfiguration configuration,
            ILogger logger)
        {
            // Options
            services.AddOptions<PaginationSetup>()
                .Bind(configuration.GetSection("PaginationSetup"))
                .ValidateOnStart();

            services.AddOptions<PriceConstsSetup>()
                .Bind(configuration.GetSection("PriceConstsSetup"))
                .ValidateOnStart();

            // MediatR
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(MediatorAssembly).Assembly));

            // Application services
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IWishListService, WishListService>();
            services.AddScoped<ICartService, CartService>();

            services.AddTransient<IUserMicroPolicy, UserMicroPolicy>();
            services.AddTransient<IRedisPolicy, RedisPolicy>();

            // Redis Configuration
            ConfigureRedis(services, configuration, logger);

            // 1. Register the REAL Redis cache (wrapped in adapter) as IRedisDistributedCache
            // This consumes the options configured in ConfigureRedis
            services.AddSingleton<IRedisDistributedCache, RedisCacheAdapter>();

            // 2. Register the Resilient Wrapper
            // It gets injected with IRedisDistributedCache (the adapter above)
            services.AddSingleton<ResilientDistributedCache>();

            // 3. Expose the Resilient Wrapper as the primary IDistributedCache
            services.AddSingleton<IDistributedCache>(sp => 
                sp.GetRequiredService<ResilientDistributedCache>());

            // HttpClient + Polly (SAFE)
            services.AddHttpClient<UserMicroClient>(client =>
            {
                client.BaseAddress = new Uri(
                    $"https://{configuration["UserMicroHostName"]}:{configuration["UserMicroPortNumber"]}");
            })
            .AddPolicyHandler((sp, _) =>
            {
                var policy = sp.GetRequiredService<IUserMicroPolicy>();
                return policy.GetCombiendAsyncPolicy();
            });

            return services;
        }

        private static void ConfigureRedis(IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            var redisConnectionString =
                $"{configuration["RedisHost"]}:{configuration["RedisPort"]}";

            var configOptions = ConfigurationOptions.Parse(redisConnectionString);

            configOptions.AbortOnConnectFail = false;
            configOptions.ConnectRetry = 2;
            configOptions.ConnectTimeout = 2000; // 2 seconds (Fast fail)
            configOptions.SyncTimeout = 2000;
            configOptions.AsyncTimeout = 2000;
            configOptions.ReconnectRetryPolicy = new LinearRetry(5000);
            configOptions.BacklogPolicy = BacklogPolicy.FailFast;
            configOptions.SocketManager = new SocketManager("Main", false);

            // Configure options for the RedisCache (used by RedisCacheAdapter)
            services.AddOptions<Microsoft.Extensions.Caching.StackExchangeRedis.RedisCacheOptions>()
                .Configure(options =>
                {
                    options.ConfigurationOptions = configOptions;
                });

            logger.LogInformation("Redis configured with fast-fail timeouts");
        }
    }
}
