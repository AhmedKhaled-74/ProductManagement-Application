using Microsoft.Extensions.Logging;
using Polly;
using StackExchange.Redis;

namespace ProductManagement.Application.Helpers.Policies
{
    public class RedisPolicy : IRedisPolicy
    {
        private readonly IAsyncPolicy _circuitBreakerPolicy;
        private readonly ILogger<RedisPolicy> _logger;

        public RedisPolicy(ILogger<RedisPolicy> logger)
        {
            _logger = logger;

            // Create circuit breaker that opens immediately on Redis connection failures
            _circuitBreakerPolicy = Policy
                .Handle<RedisConnectionException>()
                .Or<RedisTimeoutException>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 1, // Open on first failure
                    durationOfBreak: TimeSpan.FromMinutes(2), // Stay open for 2 minutes
                    onBreak: (ex, breakDelay) =>
                    {
                        _logger.LogWarning($"Redis circuit breaker opened for {breakDelay.TotalMinutes} minutes");
                    },
                    onReset: () =>
                    {
                        _logger.LogInformation("Redis circuit breaker closed");
                    },
                    onHalfOpen: () =>
                    {
                        _logger.LogInformation("Redis circuit breaker half-open - testing connection");
                    }
                );
        }

        public IAsyncPolicy GetRetryAsyncPolicy()
        {
            // Simple retry policy
            return Policy
                .Handle<RedisConnectionException>()
                .Or<RedisTimeoutException>()
                .WaitAndRetryAsync(
                    retryCount: 1,
                    sleepDurationProvider: _ => TimeSpan.FromSeconds(1)
                );
        }

        public IAsyncPolicy GetCircuitBreakerAsyncPolicy() => _circuitBreakerPolicy;

        public IAsyncPolicy GetTimeoutAsyncPolicy()
        {
            return Policy.TimeoutAsync(TimeSpan.FromSeconds(3));
        }

        public IAsyncPolicy GetCombinedAsyncPolicy()
        {
            // Just use circuit breaker for now
            return _circuitBreakerPolicy;
        }
    }
}