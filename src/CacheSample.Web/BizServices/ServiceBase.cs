using NC.InterceptorCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CacheSample.Web.BizServices
{
    /// <summary>
    /// 业务服务抽象基类
    /// </summary>
    [Cacheable]
    public abstract class ServiceBase : IService
    {
        /// <summary>
        /// 启用缓存测试方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual object GetByIdWithCache(string id)
        {
            Console.WriteLine("GetById 执行中：{0}", id);
            return new { Id = id, Name = $"Name_{id}" };
        }

        /// <summary>
        /// 忽略缓存测试方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [IgnoreCache]
        public virtual object GetByIdIgnoreCache(string id)
        {
            Console.WriteLine("IgnoreCache 执行中：{0}", id);
            return new { Id = id, Name = $"Name_{id}" };
        }
    }
}
