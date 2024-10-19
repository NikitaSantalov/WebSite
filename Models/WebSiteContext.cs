using Microsoft.EntityFrameworkCore;

namespace WebSite.Models
{
	public class WebSiteContext : DbContext
	{
		public DbSet<Customer> Customers => Set<Customer>();
		public DbSet<Product> Products => Set<Product>();
		public DbSet<Seller> Sellers => Set<Seller>();
		public DbSet<Feedback> Feedbacks => Set<Feedback>();
		public DbSet<ShoppingCart> ShoppingCart => Set<ShoppingCart>();
		public DbSet<Purchase> Purchases => Set<Purchase>();
		public DbSet<ProductImage> ProductImages => Set<ProductImage>();
		public DbSet<CustomerProduct> CustomerProducts => Set<CustomerProduct>();

		public WebSiteContext(DbContextOptions<WebSiteContext> options) : base(options)
		{
			Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
