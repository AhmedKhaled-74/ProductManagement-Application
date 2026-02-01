using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;
using ProductManagement.Application.Helpers.Policies;
using StackExchange.Redis;
using System.Text;

namespace ProductManagement.Application.Helpers
{
    /// <summary>
    /// Wrapper around IDistributedCache that applies Polly policies (retry, circuit breaker, timeout)
    /// If Redis is not running, circuit breaker will open and all operations will fail fast
    /// </summary>
    public class ResilientDistributedCache : IDistributedCache
    {
        private readonly IDistributedCache _innerCache;
        private readonly ILogger<ResilientDistributedCache> _logger;
        private readonly AsyncCircuitBreakerPolicy _circuitBreaker;
        private readonly AsyncPolicy _operationPolicy;
        private readonly AsyncPolicy _timeoutPolicy;
        private readonly IConfiguration _configuration;
        private readonly CircuitBreakerPolicy _syncCircuitBreaker;
        private readonly Policy _syncOperationPolicy;

        public ResilientDistributedCache(
            IRedisDistributedCache innerCache,
            IRedisPolicy redisPolicy,
            ILogger<ResilientDistributedCache> logger,
            IConfiguration configuration)
        {
            _innerCache = innerCache;
            _logger = logger;
            _configuration = configuration;

            // Create a timeout policy that will ALWAYS timeout after 3 seconds (User requested 3s limit)
            _timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromSeconds(3), TimeoutStrategy.Pessimistic);

            // Create circuit breaker
            _circuitBreaker = Policy
                .Handle<RedisConnectionException>()
                .Or<RedisTimeoutException>()
                .Or<TimeoutRejectedException>() // From Polly timeout
                .Or<TimeoutException>() // From System
                .Or<Exception>() // Catch-all for other Redis/Socket errors
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 1, // Open on first failure
                    durationOfBreak: TimeSpan.FromMinutes(2),
                    onBreak: (ex, breakDelay) =>
                    {
                        _logger.LogWarning($"Redis circuit breaker OPENED for {breakDelay.TotalSeconds} seconds");
                    },
                    onReset: () =>
                    {
                        _logger.LogInformation("Redis circuit breaker RESET");
                    }
                );

            // Wrap timeout with circuit breaker (CircuitBreaker -> Timeout)
            // IMPORTANT: CircuitBreaker must be OUTER so it witnesses the TimeoutRejectedException
            _operationPolicy = Policy.WrapAsync(_circuitBreaker, _timeoutPolicy);

            _syncCircuitBreaker = Policy
                .Handle<RedisConnectionException>()
                .Or<RedisTimeoutException>()
                .Or<TimeoutException>()
                .CircuitBreaker(
                    exceptionsAllowedBeforeBreaking: 1,
                    durationOfBreak: TimeSpan.FromMinutes(2)
                );

            _syncOperationPolicy = Policy
                .Timeout(TimeSpan.FromSeconds(2))
                .Wrap(_syncCircuitBreaker);


            _logger.LogInformation("ResilientDistributedCache initialized.");
        }

        // Circuit breaker status properties
        public bool IsCircuitOpen => _circuitBreaker.CircuitState == CircuitState.Open;
        public CircuitState CircuitState => _circuitBreaker.CircuitState;

        // ========== IDistributedCache Implementation ==========

        public byte[]? Get(string key)
        {
            try
            {
                return ExecuteOperation(
                () => _innerCache.Get(key),
                "Get",
                key
            );
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<byte[]?> GetAsync(string key, CancellationToken token = default)
        {
            try
            {

                return await ExecuteOperationAsync(
                    async () => await _innerCache.GetAsync(key, token),
                    "GetAsync",
                    key
                );
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            try
            {
                ExecuteOperation(
                () => { _innerCache.Set(key, value, options); return true; },
                "Set",
                key
            );
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            try
            {

                await ExecuteOperationAsync(
                async () => { await _innerCache.SetAsync(key, value, options, token); return true; },
                "SetAsync",
                key
            );
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void Refresh(string key)
        {
            try
            {
                ExecuteOperation(
                () => { _innerCache.Refresh(key); return true; },
                "Refresh",
                key
            );
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task RefreshAsync(string key, CancellationToken token = default)
        {
            try
            {
                await ExecuteOperationAsync(
                async () => { await _innerCache.RefreshAsync(key, token); return true; },
                "RefreshAsync",
                key
            );
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void Remove(string key)
        {
            try
            {
                ExecuteOperation(
                () => { _innerCache.Remove(key); return true; },
                "Remove",
                key
            );
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task RemoveAsync(string key, CancellationToken token = default)
        {
            try
            {
                await ExecuteOperationAsync(
                async () => { await _innerCache.RemoveAsync(key, token); return true; },
                "RemoveAsync",
                key
            );
            }
            catch (Exception ex)
            {
                return;
            }
        }

        // Helper methods for string operations
        public async Task<string?> GetStringAsync(string key, CancellationToken token = default)
        {
            try
            {
                var bytes = await GetAsync(key, token);
                return bytes == null ? null : Encoding.UTF8.GetString(bytes);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task SetStringAsync(string key, string value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            try
            {
                await SetAsync(key, bytes, options, token);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        // ========== Core Execution Logic ==========

        [System.Diagnostics.DebuggerNonUserCode]
        private T ExecuteOperation<T>(Func<T> operation, string operationName, string key)
        {


            try
            {
                return _syncOperationPolicy.Execute(operation);
            }
            catch
            {
                return default;
            }
        }


        [System.Diagnostics.DebuggerNonUserCode]
        private async Task<T> ExecuteOperationAsync<T>(Func<Task<T>> operation, string operationName, string key)
        {


            var result = await _operationPolicy.ExecuteAndCaptureAsync(operation);

            if (result.Outcome == OutcomeType.Successful)
            {
                return result.Result;
            }

            // Handle Failures
            if (result.FinalException is BrokenCircuitException)
            {
                _logger.LogWarning($"Redis circuit OPEN - skipping {operationName} for key: {key}");
            }
            else if (result.FinalException is TimeoutRejectedException)
            {
                _logger.LogWarning($"Redis {operationName} TIMED OUT (Polly) for key: {key}");
            }
            else if (result.FinalException is StackExchange.Redis.RedisConnectionException ex)
            {
                _logger.LogError($"Redis Connection Failed in {operationName}: {ex.Message}");
            }
            else if (result.FinalException is StackExchange.Redis.RedisTimeoutException exa)
            {
                _logger.LogError($"Redis Timeout in {operationName}: {exa.Message}");
            }
            else
            {
                _logger.LogError(result.FinalException, $"Unexpected error in Redis {operationName} for key: {key}");
            }

            return default(T);
        }
    }
}
