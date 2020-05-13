using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;

namespace NC.InterceptorCache
{
    /// <summary>
    /// 自定义缓存接口
    /// TODO...
    /// </summary>
    public interface ICaching // : IDistributedCache
    {
        object Get(string cacheKey);
        void Set(string cacheKey, object cacheValue);

        // 完善缓存方法
        //object Get(object key);
        //TItem Get<TItem>(object key);
        //object GetOrCreate(object key);
        //object GetOrCreateAsync(object key);
        //void Set<TItem>(object key, TItem item);
        //void Set<TItem>(object key, TItem item);
    }
}
