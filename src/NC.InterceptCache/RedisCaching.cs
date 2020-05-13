using System;
using System.Collections.Generic;
using System.Text;

namespace NC.InterceptorCache
{
    /// <summary>
    /// Redis 缓存
    /// TODO...
    /// </summary>
    public class RedisCaching : ICaching
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
