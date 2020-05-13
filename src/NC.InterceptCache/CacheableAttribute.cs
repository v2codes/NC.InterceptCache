using System;
using System.Collections.Generic;
using System.Text;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;

namespace NC.InterceptorCache
{
    /// <summary>
    /// 启用缓存标记
    /// 该标记限制用于类、接口，用于拦截内部虚方法成员并加入缓存逻辑
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class CacheableAttribute : InterceptAttribute
    {
        /// <summary>
        /// 过期时间
        /// </summary>
        public TimeSpan ExpireTimeSpan { get; private set; }

        public CacheableAttribute()
           : base(new TypedService(typeof(CacheableInterceptor)))
        {
        }                                                                  

        public CacheableAttribute(TimeSpan expireTimeSpan)
            : base(new TypedService(typeof(CacheableInterceptor)))
        {
            ExpireTimeSpan = expireTimeSpan;
        }
    }

    /// <summary>
    /// 忽略缓存标记
    /// 该标记用于具体方法，当方法所在类或接口使用了 CacheableAttribute 时，此标记表示当前所修饰的方法会忽略缓存。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class IgnoreCacheAttribute : Attribute
    {

    }
}
