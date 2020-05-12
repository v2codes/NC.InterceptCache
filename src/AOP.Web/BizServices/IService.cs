using AOP.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOP.Web.BizServices
{
    //[Cacheable]
    public interface IService
    {
        object GetByIdWithCache(string id);
        object GetByIdIgnoreCache(string id);
    }
}
