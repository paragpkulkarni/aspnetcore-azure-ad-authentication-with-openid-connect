using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;

namespace aspnetcore_with_openid_connect.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        IWebHostEnvironment env;
        private readonly ILogger<WeatherForecastController> _logger;
        readonly ITokenAcquisition tokenAcquisition;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWebHostEnvironment env  
             , ITokenAcquisition tokenAcquisition
            )
        {
            _logger = logger;
            this.env = env;
            this.tokenAcquisition = tokenAcquisition;
        }

        [HttpGet]
        public async Task<string> Get()
        {
           

            var httpClient = new HttpClient();
           
            
            var token = "";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); 
            return "hello world";
        }

        [HttpGet]
        [Route("hello")]
        public async Task<string> Hello()
        {
            string[] scopes = new string[] { "" };
            string accessToken = await tokenAcquisition.GetAccessTokenForUserAsync(scopes);
            return "hello world";
        }
    }
}
