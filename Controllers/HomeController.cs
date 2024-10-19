using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebSite.Extensions;

namespace WebSite.Controllers
{
	[Controller]
	[Route("/")]
    public class HomeController : Controller
	{
		private readonly IConfiguration _config;

		public HomeController(IConfiguration config)
		{
			_config = config;
		}

		[HttpGet]
		[Route("/")]
		public void Index()
		{
			HttpContext.Response.SendHtmlFile(_config.GetHomePage());
		}
	}
}
