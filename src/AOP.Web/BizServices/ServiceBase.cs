using AOP.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOP.Web.BizServices
{
    [Cacheable]
    public abstract class ServiceBase : IService
    {
        public virtual object GetByIdWithCache(string id)
        {
            Console.WriteLine("GetById 执行中：{0}", id);
            return new { Id = id, Name = $"Name_{id}" };
        }

        [IgnoreCache]
        public virtual object GetByIdIgnoreCache(string id)
        {
            Console.WriteLine("IgnoreCache 执行中：{0}", id);
            return new { Id = id, Name = $"Name_{id}" };
        }
    }
}
