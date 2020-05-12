using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AOP.Cache;
using AOP.Web.BizServices;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AOP.Web.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IService _service;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IService service,ILogger<WeatherForecastController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var testKey = Guid.NewGuid();
            var testString = _service.GetByIdWithCache(testKey.ToString());
            //var testString2 = _service.GetByIdIgnoreCache(testKey.ToString());

            Console.WriteLine("GetByIdWithCache：", testString);
            //Console.WriteLine("GetByIdIgnoreCache：", testString2);

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("cache")]
        public IActionResult TestCache([FromQuery]string id)
        {
            var testKey = Guid.NewGuid();
            var testString = _service.GetByIdWithCache(testKey.ToString());
            //var testString2 = _service.GetByIdIgnoreCache(testKey.ToString());

            Console.WriteLine("GetByIdWithCache：", testString);
            //Console.WriteLine("GetByIdIgnoreCache：", testString2);
            return Ok(testString);
        }
    }
}
