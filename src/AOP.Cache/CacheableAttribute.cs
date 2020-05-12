using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace AOP.Cache
{
    /// <summary>
    /// 启用AOP缓存
    /// //TODO...
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class CacheableAttribute : InterceptAttribute
    {
        public CacheableAttribute()
           : base(new TypedService(typeof(CacheableInterceptor)))
        {
        }
    }

    /// <summary>
    /// 忽略缓存
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class IgnoreCacheAttribute : Attribute
    {

    }
}
