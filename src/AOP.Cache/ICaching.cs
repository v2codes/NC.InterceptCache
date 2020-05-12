using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;

namespace AOP.Cache
{
    /// <summary>
    /// 自定义缓存
    /// </summary>
    public interface ICaching // : IDistributedCache
    {
        object Get(string cacheKey);
        void Set(string cacheKey, object cacheValue);
    }
}
