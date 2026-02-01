using Microsoft.Extensions.Logging;
using ProductManagement.Application.Helpers.Policies;
using StackExchange.Redis;

namespace ProductManagement.Application.Helpers
{
    /// <summary>
    /// Factory for creating Redis connections with Polly policies applied
    /// </summary>
    public class ResilientRedisConnectionFactory
    {
        private readonly IRedisPolicy _redisPolicy;
        private readonly ILogger<ResilientRedisConnectionFactory> _logger;

        public ResilientRedisConnectionFactory(
            IRedisPolicy redisPolicy,
            ILogger<ResilientRedisConnectionFactory> logger)
        {
            _redisPolicy = redisPolicy;
            _logger = logger;
        }

        /// <summary>
        /// Creates a ConnectionMultiplexer with circuit breaker, retry, and timeout policies applied
        /// </summary>
        public async Task<IConnectionMultiplexer> CreateConnectionAsync(ConfigurationOptions configOptions)
        {
            var policy = _redisPolicy.GetCombinedAsyncPolicy();
            
            return await policy.ExecuteAsync(async () =>
            {
                _logger.LogInformation("Attempting to establish Redis connection with policies applied...");
                var connection = await ConnectionMultiplexer.ConnectAsync(configOptions);
                _logger.LogInformation("Redis connection established successfully");
                return connection;
            });
        }

        /// <summary>
        /// Creates a ConnectionMultiplexer synchronously with policies applied
        /// </summary>
        public IConnectionMultiplexer CreateConnection(ConfigurationOptions configOptions)
        {
            var policy = _redisPolicy.GetCombinedAsyncPolicy();
            
            return policy.ExecuteAsync(async () =>
            {
                _logger.LogInformation("Attempting to establish Redis connection with policies applied...");
                var connection = await ConnectionMultiplexer.ConnectAsync(configOptions);
                _logger.LogInformation("Redis connection established successfully");
                return connection;
            }).GetAwaiter().GetResult();
        }
    }
}

