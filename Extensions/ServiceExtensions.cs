using Microsoft.Extensions.DependencyInjection;
using WebSite.Models;
using WebSite.Repositoryes.Interfaces;
using WebSite.Repositoryes;
using WebSite.Services.Interfaces;
using WebSite.Services;

namespace WebSite.Extensions
{
	public static class ServiceExtensions
	{
		public static void AddRepository(this IServiceCollection services)
		{
			services.AddTransient<IRepository<Seller>, WebSiteRepository<Seller>>();
			services.AddTransient<IRepository<Customer>, WebSiteRepository<Customer>>();
			services.AddTransient<IRepository<Product>, WebSiteRepository<Product>>();
			services.AddTransient<IRepository<Feedback>, WebSiteRepository<Feedback>>();
			services.AddTransient<IRepository<Purchase>, WebSiteRepository<Purchase>>();
			services.AddTransient<IRepository<ProductImage>, WebSiteRepository<ProductImage>>();
			services.AddTransient<IRepository<ShoppingCart>, WebSiteRepository<ShoppingCart>>();
		}

		public static void AddServices(this IServiceCollection services)
		{
			services.AddTransient<IValidationService, ValidationService>();
			services.AddTransient<IImageService, LocalImageService>();
		}
	}
}
