using Microsoft.AspNetCore.Http;

namespace WebSite.Extensions
{
	public static class ResponseExtensions
	{
		public static void SendHtmlFile(this HttpResponse response, string filePath)
		{
			response.ContentType = "text/html";
			response.SendFileAsync(filePath);
		}
	}
}
