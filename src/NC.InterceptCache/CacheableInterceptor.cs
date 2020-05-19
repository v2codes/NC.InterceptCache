using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using NC.InterceptorCache.Attributes;

namespace NC.InterceptorCache
{
    /// <summary>
    /// 缓存拦截器
    /// 用于拦截方法，在具体方法执行前，如果存在缓存数据则直接返回缓存的数据，如果没有缓存则执行具体方法，并将方法返回值写入缓存中
    /// </summary>       
    public class CacheableInterceptor : IInterceptor
    {
        // 内存缓存
        private readonly IMemoryCache _memoryCache;

        // 分布式缓存
        private readonly IDistributedCache _distributedCache;

        public CacheableInterceptor(IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }

        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("执行前...");

            // 判断当前方法是否包含 IgnoreCache 特性
            var method = invocation.GetConcreteMethod(); // 调用的具体方法，接口、具体类
            var targetMethod = invocation.GetConcreteMethodInvocationTarget(); // 获取具体的调用目标方法，也就是实际执行的方法

            // 忽略缓存
            var ignoreAttr = this.GetAttribute<IgnoreCacheAttribute>(method, targetMethod);
            if (ignoreAttr != null)
            {
                Console.WriteLine("忽略缓存...");
                // 执行原本方法
                invocation.Proceed();
                return;
            }

            Console.WriteLine("读取本地缓存...");
            // 本地缓存配置
            var memoryOptions = this.GetAttribute<MemoryCacheOptionsAttribute>(method, targetMethod);
            // 生成缓存 key
            var memoryCacheKey = this.GenericCacheKey(invocation, memoryOptions.KeyPrefix);

            // Redis缓存获取
            var redisOptions = this.GetAttribute<RedisCacheOptionsAttribute>(method, targetMethod);

            // 读取本地缓存
            var cacheValue = _memoryCache.Get(memoryCacheKey);

            if (cacheValue == null)
            {
                Console.WriteLine("读取Redis缓存...");
                // 生成缓存 key
                var redisCacheKey = this.GenericCacheKey(invocation, redisOptions.KeyPrefix);
                cacheValue = _distributedCache.Get(redisCacheKey);
            }

            if (cacheValue != null)
            {
                Console.WriteLine("读取缓存...");
                //将当前获取到的缓存值，赋值给当前执行方法
                invocation.ReturnValue = cacheValue;
                return;
            }

            // 执行原本方法
            invocation.Proceed();

            //存入缓存
            if (!string.IsNullOrWhiteSpace(memoryCacheKey))
            {

                Console.WriteLine("写入本地缓存...");
                // 写入本地缓存
                var entryOptions = new MemoryCacheEntryOptions()
                                   .SetSize(1)
                                   // (TimeSpan.FromSeconds((double)memoryOptions.AbsoluteExpiration))
                                   .SetSlidingExpiration(TimeSpan.FromSeconds((double)memoryOptions.SlidingExpiration));
                if (memoryOptions.AbsoluteExpiration > 0)
                {
                    entryOptions.SetAbsoluteExpiration(DateTime.Now.AddSeconds((double)memoryOptions.AbsoluteExpiration));
                }
                _memoryCache.Set(memoryCacheKey, invocation.ReturnValue, entryOptions);

                Console.WriteLine("写入Redis缓存...");
                // 写入Redis缓存
                var redisEntryOptions = new DistributedCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromSeconds((double)redisOptions.SlidingExpiration)
                };
                if (redisOptions.AbsoluteExpiration > 0)
                {
                    redisEntryOptions.SetAbsoluteExpiration(DateTime.Now.AddSeconds((double)redisOptions.AbsoluteExpiration));
                }
                _distributedCache.SetString(memoryCacheKey, System.Text.Json.JsonSerializer.Serialize(invocation.ReturnValue), redisEntryOptions);

            }
            Console.WriteLine("执行后...");
        }

        /// <summary>
        /// 获取方法指定特性
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="method">具体方法，接口、具体实现类中的定义</param>
        /// <param name="targetMethod">获取具体的调用目标方法，具体实际执行的方法，如基类中的虚方法</param>
        /// <returns></returns>
        private TAttribute GetAttribute<TAttribute>(MethodInfo method, MethodInfo targetMethod) where TAttribute : Attribute
        {
            var attribute = method.GetCustomAttribute<TAttribute>();
            if (attribute == null)
            {
                attribute = targetMethod.GetCustomAttribute<TAttribute>();
            }
            return attribute;
        }

        /// <summary>
        /// 生成缓存Key
        /// </summary>
        /// <param name="invocation">拦截装载对象</param>
        /// <param name="keyPrefix">Key前缀</param>
        /// <returns></returns>
        private string GenericCacheKey(IInvocation invocation, string keyPrefix)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            // 获取参数列表
            var methodArguments = invocation.Arguments.Select(GetArgumentValue).ToList();

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}{1}:{2}", keyPrefix, typeName, methodName);
            methodArguments.ForEach(arg =>
            {
                sb.AppendFormat(":{0}", arg);
            });
            return sb.ToString();
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private string GetArgumentValue(object arg)
        {
            if (arg is int || arg is long || arg is string)
                return arg.ToString();

            if (arg is DateTime)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            return "";
        }
    }
}


// 基于 abp 拦截器的缓存key生成示例
///// <summary>
///// 缓存 Key 生成工具（IAbpMethodInvocation 扩展）
///// </summary>
//public static class CacheKeyGeneratorExtension
//{
//    // 分隔符
//    private static string Separator = ":";

//    /// <summary>
//    /// 生成缓存 Key
//    /// </summary>
//    /// <param name="invocation">拦截装载对象</param>
//    /// <param name="keyPrefix">自定义 Key 前缀</param>
//    /// <returns></returns>
//    public static string GetCacheKey(this IAbpMethodInvocation invocation, string keyPrefix)
//    {
//        var typeName = invocation.Method.DeclaringType?.Name;
//        var methodName = invocation.Method.Name;

//        if (!string.IsNullOrWhiteSpace(keyPrefix))
//        {
//            return $"{keyPrefix}{Separator}";
//        }

//        return $"{typeName}{Separator}{methodName}{Separator}";
//    }

//    /// <summary>
//    /// 生成缓存 Key
//    /// 方法参数参与生成该Key，默认取3个参数
//    /// </summary>
//    /// <param name="invocation">拦截装载对象</param>
//    /// <param name="keyPrefix">自定义 Key 前缀</param>
//    /// <returns></returns>
//    public static string GetCacheKeyWithArgs(this IAbpMethodInvocation invocation, string keyPrefix)
//    {
//        var arguments = invocation.Arguments.Take(5).ToArray();
//        return invocation.GetCacheKey(keyPrefix, arguments);
//    }

//    /// <summary>
//    /// 生成缓存 Key
//    /// </summary>
//    /// <param name="invocation">拦截装载对象</param>
//    /// <param name="keyPrefix">前缀</param>
//    /// <param name="parameters">自定义前缀字符串数组，使用分隔符分割并拼接</param>
//    /// <returns></returns>
//    public static string GetCacheKey(this IAbpMethodInvocation invocation, string keyPrefix, IEnumerable<string> parameters)
//    {
//        var cacheKeyPrefix = invocation.GetCacheKey(keyPrefix);

//        var builder = new StringBuilder();
//        builder.Append(cacheKeyPrefix);
//        builder.Append(string.Join(Separator, parameters));
//        return builder.ToString();
//    }

//    /// <summary>
//    /// 生成缓存 Key
//    /// </summary>
//    /// <param name="invocation">拦截装载对象</param>
//    /// <param name="prefix">前缀</param>
//    /// <param name="args">自定义前缀字对象数组，视情况取值或转换后，使用分隔符分割并拼接</param>
//    /// <returns></returns>
//    public static string GetCacheKey(this IAbpMethodInvocation invocation, string prefix, object[] args)
//    {
//        var method = invocation.Method;
//        var methodArguments = args?.Any() == true
//                                  ? args.Select(GenerateCacheKey)
//                                  : new[] { "0" };

//        return invocation.GetCacheKey(prefix, methodArguments);
//    }

//    /// <summary>
//    /// 格式化参数
//    /// </summary>
//    /// <param name="parameter"></param>
//    /// <returns></returns>
//    private static string GenerateCacheKey(object parameter)
//    {
//        if (parameter == null) return string.Empty;
//        //if (parameter is ICachable cachable) return cachable.CacheKey;
//        if (parameter is string key) return key;
//        if (parameter is DateTime dateTime) return dateTime.ToString("O");
//        if (parameter is DateTimeOffset dateTimeOffset) return dateTimeOffset.ToString("O");
//        if (parameter is IEnumerable enumerable) return GenerateCacheKey(enumerable.Cast<object>());
//        return parameter.ToString();
//    }

//    /// <summary>
//    /// 格式化列表类型参数
//    /// </summary>
//    /// <param name="parameter"></param>
//    /// <returns></returns>
//    private static string GenerateCacheKey(IEnumerable<object> parameter)
//    {
//        if (parameter == null) return string.Empty;
//        return "[" + string.Join(",", parameter) + "]";
//    }
//}