using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NC.InterceptorCache;
using CacheSample.Web.BizServices;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace CacheSample.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ILifetimeScope AutofacContainer { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOptions();

            // 启用内存缓存
            services.AddMemoryCaching(new MemoryCacheOptions()
            {
                // SizeLimit = 1024 // 设置缓存的最大大小，设置了该值时，写入缓存是必须显示指定缓存大小
                // ExpirationScanFrequency= TimeSpan.FromSeconds(5), // 设置扫描过期项的时间间隔
                // CompactionPercentage=1024, // 设置在超出最大大小时要压缩的缓存量
            });

            // 启用Redis
            services.AddRedisCache();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // 
            builder.RegisterType<MemoryCaching>().As<ICaching>();
            //builder.RegisterType<MemoryCache>().As<IMemoryCache>().SingleInstance();

            // TODO：注册Redis缓存

            builder.RegisterType<TestService>().As<IService>().EnableInterfaceInterceptors();
            builder.RegisterType<CacheableInterceptor>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime, IDistributedCache cache)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            lifetime.ApplicationStarted.Register(() =>
            {
                var currentTime = DateTime.Now.ToString();
                byte[] encodedCurrentTime = Encoding.UTF8.GetBytes(currentTime);
                var options = new DistributedCacheEntryOptions()
                              .SetSlidingExpiration(TimeSpan.FromSeconds(20));
                cache.Set("cachedTime", encodedCurrentTime, options);
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
