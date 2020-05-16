using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CacheSample.Web.BizServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CacheSample.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IService _service;

        private readonly ILogger<TestController> _logger;

        public TestController(IService service, ILogger<TestController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// 缓存测试
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("cache/{id}")]
        public IActionResult Cache(string id)
        {
            var testKey = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            var testString = _service.GetByIdWithCache(testKey.ToString());
            //var testString2 = _service.GetByIdIgnoreCache(testKey.ToString());

            Console.WriteLine("GetByIdWithCache：{0}", JsonSerializer.Serialize(testString));
            //Console.WriteLine("GetByIdIgnoreCache：", testString2);
            return Ok(testString);
        }
    }
}
