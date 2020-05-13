using NC.InterceptorCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CacheSample.Web.BizServices
{
    /// <summary>
    /// 业务服务接口
    /// </summary>
    //[Cacheable]
    public interface IService
    {
        /// <summary>
        /// 启用缓存测试方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        object GetByIdWithCache(string id);

        /// <summary>
        /// 忽略缓存测试方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        object GetByIdIgnoreCache(string id);
    }
}
