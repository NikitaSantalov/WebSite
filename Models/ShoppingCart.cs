
namespace WebSite.Models
{
	public record ShoppingCart : Entity
	{
		public int ProductId { get; set; }
		public int CustomerId { get; set; }
		public int Count { get; set; }
		public string Date { get; set; } = string.Empty;
	}
}
