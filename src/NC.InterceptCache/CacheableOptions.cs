using System;
using System.Collections.Generic;
using System.Text;

namespace NC.InterceptorCache.Attributes
{
    /// <summary>
    /// 缓存配置项
    /// </summary>
    internal class CacheableOption// : Attribute
    {
        /// <summary>
        /// 自定义Key前缀
        /// </summary>
        public string KeyPrefix { get; private set; }

        /// <summary>
        /// 自定义Key前缀
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 绝对过期时间
        /// </summary>
        public DateTime? AbsoluteExpiration { get; private set; } = DateTime.Now.AddMilliseconds(10);

        /// <summary>
        /// 滑动过期时长
        /// 可变过期时间，当在此期间，缓存被触发时会重置过期时间
        /// </summary>
        public TimeSpan SlidingExpiration { get; private set; } = TimeSpan.FromMilliseconds(10);

        /// <summary>
        /// 启用内存缓存
        /// </summary>
        public CacheableOption()
        {
        }

        /// <summary>
        /// 启用缓存拦截器
        /// </summary>
        /// <param name="slidingExpiration">滑动过期时长</param>
        /// <param name="keyPrefix">自定义缓存Key前缀</param>
        /// <param name="description">描述</param>
        public CacheableOption(TimeSpan slidingExpiration, string keyPrefix = null, string description = null)
        {
            SlidingExpiration = slidingExpiration;
            KeyPrefix = keyPrefix;
            Description = description;
        }

        /// <summary>
        /// 启用缓存拦截器
        /// </summary>
        /// <param name="expireTime">绝对过期时间</param>
        /// <param name="keyPrefix">自定义缓存Key前缀</param>
        /// <param name="description">描述</param>
        public CacheableOption(DateTime expireTime, string keyPrefix = null, string description = null)
        {
            AbsoluteExpiration = expireTime;
            KeyPrefix = keyPrefix;
            Description = description;
        }
    }
}
