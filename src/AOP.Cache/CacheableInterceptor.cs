using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AOP.Cache
{
    /// <summary>
    /// 缓存拦截器
    /// </summary>
    public class CacheableInterceptor : IInterceptor
    {
        private readonly ICaching _cache;

        public CacheableInterceptor(ICaching caching)
        {
            _cache = caching;
        }

        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("执行前...");

            // 判断当前方法是否包含 IgnoreCache 特性
            var method = invocation.GetConcreteMethod();
            var attributes = method.GetCustomAttribute<IgnoreCacheAttribute>();
            if (attributes != null)
            {
                Console.WriteLine("忽略缓存");
                // 执行原本方法
                invocation.Proceed();
            }
            else
            {
                // 获取自定义缓存
                var cacheKey = GenericCacheKey(invocation);

                // 根据key获取相应的缓存值
                var cacheValue = _cache.Get(cacheKey);

                if (cacheValue != null)
                {
                    //将当前获取到的缓存值，赋值给当前执行方法
                    invocation.ReturnValue = cacheValue;
                    return;
                }

                // 执行原本方法
                invocation.Proceed();

                //存入缓存
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    _cache.Set(cacheKey, invocation.ReturnValue);
                }
            }
            Console.WriteLine("执行后...");
        }

        private string GenericCacheKey(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            // 获取参数列表
            var methodArguments = invocation.Arguments.Select(GetArgumentValue).ToList();

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}:{1}", typeName, methodName);
            methodArguments.ForEach(arg =>
            {
                sb.AppendFormat(":{0}", arg);
            });
            return sb.ToString();
        }

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
