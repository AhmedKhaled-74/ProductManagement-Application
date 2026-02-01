using Microsoft.Extensions.Logging;
using Polly;

namespace ProductManagement.Application.Helpers.Policies
{
    public class UserMicroPolicy(ILogger<UserMicroPolicy> logger) : IUserMicroPolicy
    {
        public IAsyncPolicy<HttpResponseMessage> GetRetryAsyncPolicy()
        {
            var policy =
                Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode
                && (r.StatusCode == System.Net.HttpStatusCode.InternalServerError
                || r.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable))
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        // Optionally log each retry attempt
                        //logger.LogWarning($"Delaying for {timespan.TotalSeconds} seconds, then making retry {retryAttempt}.");
                        //logger.LogWarning($"Request failed with {outcome.Result.StatusCode}. Waiting {timespan} before next retry. Retry attempt {retryAttempt}.");
                    }
                );
            return policy;
        }
        public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerAsyncPolicy()
        {
            var policy =
                 Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode
                 && (r.StatusCode == System.Net.HttpStatusCode.InternalServerError
                 || r.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable))
                 .CircuitBreakerAsync(
                     handledEventsAllowedBeforeBreaking: 2,
                     durationOfBreak: TimeSpan.FromMinutes(2),
                     onBreak: (outcome, timespan) =>
                     {
                         logger.LogInformation($"cb opened for {timespan.TotalMinutes}");
                     },
                     onReset: () =>
                     {
                         logger.LogInformation($"cb closed");
                     });

            return policy;
        }
        public IAsyncPolicy<HttpResponseMessage> GetTimeoutAsyncPolicy()
        {
            var policy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMilliseconds(5000));
            return policy;
        }
        public IAsyncPolicy<HttpResponseMessage> GetCombiendAsyncPolicy()
        {
            var retryPolicy = GetRetryAsyncPolicy();
            var circuitBreakerPolict = GetCircuitBreakerAsyncPolicy();
            var timeoutPolicy = GetTimeoutAsyncPolicy();
            var wrappedPolicy = Policy.WrapAsync(retryPolicy, circuitBreakerPolict, timeoutPolicy);
            return wrappedPolicy;
        }

    }
}
