using System;
using System.Collections.Generic;
using System.Text;

namespace NC.InterceptorCache.Attributes
{
    public class RedisCacheOptionsAttribute : Attribute
    {
        /// <summary>
        /// 自定义Key前缀
        /// </summary>
        public string KeyPrefix { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 绝对过期时长（秒）
        /// </summary>
        public int? AbsoluteExpiration { get; private set; } = 0;

        /// <summary>
        /// 滑动过期时长（秒）
        /// 可变过期时间，当在此期间，缓存被触发时会重置过期时间
        /// </summary>
        public int? SlidingExpiration { get; private set; } = 10;

        /// <summary>
        /// 启用内存缓存
        /// </summary>
        public RedisCacheOptionsAttribute()
        {
        }

        /// <summary>
        /// 启用缓存拦截器
        /// </summary>
        /// <param name="slidingExpiration">滑动过期时长：秒，默认10，当绝对过期时长为0时，该值有效</param>
        /// <param name="absoluteExpiration">绝对过期时长：秒，当绝对过期时长不为0时，超过该时间缓存必定过期</param>
        /// <param name="keyPrefix">自定义缓存Key前缀</param>
        /// <param name="description">描述</param>
        public RedisCacheOptionsAttribute(int slidingExpiration = 10, int absoluteExpiration = 0, string keyPrefix = null, string description = null)
        {
            AbsoluteExpiration = absoluteExpiration;
            SlidingExpiration = slidingExpiration;
            KeyPrefix = keyPrefix;
            Description = description;
        }
    }
}
