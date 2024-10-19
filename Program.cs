using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using WebSite.Extensions;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebSite
{
    public class Program
	{
		public static void Main(string[] args)
		{
			Startup startup = new Startup();

			var app = startup.BuildApplication(args);

			app.Urls.Add(app.Configuration.GetUrl());

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.MapControllers();
			app.UseAuthorization();
			app.UseStaticFiles(new StaticFileOptions()
			{
				OnPrepareResponse = ctx =>
				{
					ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=60");
				}
			});

			app.Run();
		}
	}
}
