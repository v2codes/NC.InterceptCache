using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace NC.InterceptorCache
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 启用内存缓存，基于内置 MemoryCache
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddMemoryCaching(this IServiceCollection services, MemoryCacheOptions options)
        {
            // 自定义缓存
            // services.AddSingleton<ICaching, MemoryCaching>();
            services.AddSingleton<IMemoryCache>(factory =>
            {
                var cache = new MemoryCache(options);
                return cache;
            });
            return services;
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost";
                options.InstanceName = "TestCache";
            });
            return services;
        }
    }
}
