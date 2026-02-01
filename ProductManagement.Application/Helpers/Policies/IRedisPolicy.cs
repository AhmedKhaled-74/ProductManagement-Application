using Polly;

namespace ProductManagement.Application.Helpers.Policies
{
    public interface IRedisPolicy
    {
        IAsyncPolicy GetRetryAsyncPolicy();
        IAsyncPolicy GetCircuitBreakerAsyncPolicy();
        IAsyncPolicy GetTimeoutAsyncPolicy();
        IAsyncPolicy GetCombinedAsyncPolicy();
    }
}

