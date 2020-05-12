using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AOP.Cache
{
    /// <summary>
    /// 内存缓存
    /// </summary>
    public class MemoryCaching : ICaching
    {
        public readonly IMemoryCache _cache;
        public MemoryCaching(IMemoryCache cache)
        {
            _cache = cache;
        }
        public object Get(string cacheKey)
        {
            return _cache.Get(cacheKey);
        }

        public void Set(string cacheKey, object cacheValue)
        {
            _cache.Set(cacheKey, cacheValue);
        }
    }
}
