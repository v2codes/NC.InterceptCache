using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace NC.InterceptorCache
{
    public static class ServiceCollectionExtensions
    {
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
    }
}
