using Microsoft.AspNetCore.Mvc;

namespace PlantDataAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public ContentResult Get()
        {
            string content = "<h1>Plant Data API</h1>" +
                             "<a href=\"/docs\">API Docs</a>" +
                             "<a href=\"/\">Github</a>";

            return new ContentResult()
            {
                ContentType = "text/html",
                Content = content
            };
        }
    }
}