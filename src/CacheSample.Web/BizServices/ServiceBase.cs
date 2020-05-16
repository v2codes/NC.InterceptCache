using System;
using NC.InterceptorCache.Attributes;

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
        [MemoryCacheOptions(10, -1, "m_", "描述balabala....")]
        [RedisCacheOptions(10, -1, "r_", "描述balabala....")]
        public virtual object GetByIdWithCache(string id)
        {
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
            return new { Id = id, Name = $"Name_{id}" };
        }
    }
}
