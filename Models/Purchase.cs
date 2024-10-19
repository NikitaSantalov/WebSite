
namespace WebSite.Models
{
	public record Purchase : Entity
	{
		public int ProductId { get; set; }
		public int CustomerId { get; set; }
		public int Count { get; set; }
		public string Date { get; set; } = string.Empty;
	}
}
