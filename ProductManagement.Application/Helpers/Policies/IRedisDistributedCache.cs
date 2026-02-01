using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Helpers.Policies
{
    public interface IRedisDistributedCache : IDistributedCache
    {
    }
}
