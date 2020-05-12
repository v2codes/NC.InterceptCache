using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AOP.Cache
{
    public class RedisCache : ICaching
    {
        public object Get(string cacheKey)
        {
            string content = $"Redis 假装已获取 ，Key ={cacheKey}";
            Console.WriteLine(content);
            return content;
        }

        public void Set(string cacheKey, object cacheValue)
        {
            string content = $"Redis 假装已保存 ，Key ={cacheKey}";
            Console.WriteLine(content);
        }
    }
}
