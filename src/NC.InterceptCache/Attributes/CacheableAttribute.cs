using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace NC.InterceptorCache.Attributes
{
    /// <summary>
    /// 启用缓存标记
    /// 该标记限制用于类、接口，用于拦截内部虚方法成员并加入缓存逻辑
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class CacheableAttribute : InterceptAttribute
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
        /// 启用缓存拦截器
        /// </summary>
        public CacheableAttribute()
           : base(new TypedService(typeof(CacheableInterceptor)))
        {
        }

        /// <summary>
        /// 启用缓存拦截器
        /// </summary>
        /// <param name="slidingExpiration">滑动过期时长：秒，默认10，当绝对过期时长为0时，该值有效</param>
        /// <param name="absoluteExpiration">绝对过期时长：秒，当绝对过期时长不为0时，超过该时间缓存必定过期</param>
        /// <param name="keyPrefix">自定义缓存Key前缀</param>
        /// <param name="description">描述</param>
        public CacheableAttribute(int slidingExpiration = 10, int absoluteExpiration = 0, string keyPrefix = null, string description = null)
            : base(new TypedService(typeof(CacheableInterceptor)))
        {
            AbsoluteExpiration = absoluteExpiration;
            SlidingExpiration = slidingExpiration;
            KeyPrefix = keyPrefix;
            Description = description;
        }
    }
}
