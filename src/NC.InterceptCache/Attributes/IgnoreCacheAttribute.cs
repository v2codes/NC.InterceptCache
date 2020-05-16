using System;
using System.Collections.Generic;
using System.Text;

namespace NC.InterceptorCache.Attributes
{
    /// <summary>
    /// 忽略缓存标记
    /// 当方法所在类或接口使用了 CacheableAttribute 时，该标记作用于具体方法，用于忽略缓存。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class IgnoreCacheAttribute : Attribute
    {

    }
}
