using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebSite.Extensions;
using WebSite.Models;
using WebSite.Options;

namespace WebSite
{
	public class Startup
	{
		public WebApplication BuildApplication(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Environment.EnvironmentName = "Development";

			if (builder.Environment.IsDevelopment())
			{
				builder.Configuration.AddJsonFile("Configuration/appsettings.Development.json");
			}
			else
			{
				builder.Configuration.AddJsonFile("Configuration/appsettings.json");
			}

			builder.Configuration.AddJsonFile("Configuration/views.json");
			builder.Configuration.AddJsonFile("Configuration/admin.json");

			string connection = builder.Configuration.GetConnectionString("DefaultConnection")!;

			builder.Services.AddDbContext<WebSiteContext>(options => options.UseNpgsql(connection));

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(SetJwtBearerOptions);

			builder.Services.AddServices();
			builder.Services.AddDtoServices();
			builder.Services.AddRepository();
			builder.Services.AddAuthorization();
			builder.Services.AddControllers();
			builder.Services.AddSwaggerGen(SetSwaggerGenOptions);

			return builder.Build();
		}

		private void SetSwaggerGenOptions(SwaggerGenOptions options)
		{
			options.SwaggerDoc("v1", new OpenApiInfo { Title = "WebSite", Version = "v1" });

			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				In = ParameterLocation.Header,
				Description = "Please enter token",
				Name = "Authorization",
				Type = SecuritySchemeType.Http,
				BearerFormat = "JWT",
				Scheme = "bearer"
			});

			options.AddSecurityRequirement(new OpenApiSecurityRequirement()
			{
				{
					new OpenApiSecurityScheme()
					{
						Reference = new OpenApiReference
						{
							Type=ReferenceType.SecurityScheme,
							Id="Bearer"
						}
					},
					new string[]{}
				}
			});
		}

		private void SetJwtBearerOptions(JwtBearerOptions options)
		{
			options.TokenValidationParameters = new TokenValidationParameters()
			{
				ValidateIssuer = true,
				ValidIssuer = AuthOptions.ISSUER,
				ValidateAudience = true,
				ValidAudience = AuthOptions.AUDIENCE,
				ValidateLifetime = true,
				IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
				ValidateIssuerSigningKey = true
			};
		}
	}
}
