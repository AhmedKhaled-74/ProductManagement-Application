using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Helpers.Policies
{
    public interface IUserMicroPolicy
    {
         
        public IAsyncPolicy<HttpResponseMessage> GetRetryAsyncPolicy();
        public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerAsyncPolicy();
        public IAsyncPolicy<HttpResponseMessage> GetTimeoutAsyncPolicy();
        public IAsyncPolicy<HttpResponseMessage> GetCombiendAsyncPolicy();
    }
}
