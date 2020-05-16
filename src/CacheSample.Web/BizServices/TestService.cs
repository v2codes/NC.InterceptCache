using NC.InterceptorCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NC.InterceptorCache.Attributes;

namespace CacheSample.Web.BizServices
{
    ///// <summary>
    ///// 测试业务服务实现类，可直接实现接口
    ///// </summary>
    //[Cacheable]
    //public class TestService : IService
    //{
    //}

    /// <summary>
    /// 测试业务服务实现类，继承基类
    /// </summary>
    //[Cacheable]
    public class TestService : ServiceBase 
    {
        ///// <summary>
        ///// 启用缓存测试方法
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[MemoryCacheOptions(10, -1, "Test_", "描述balabala....")]
        //public override object GetByIdWithCache(string id)
        //{
        //    Console.WriteLine("GetById 执行中：{0}", id);
        //    return new { Id = id, Name = $"Name_{id}" };
        //}
    }
}
