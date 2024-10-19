namespace WebSite.Models
{
	public record Feedback : Entity
	{
		public int ProductId { get; set; }
		public int CustomerId { get; set; }
		public string Text { get; set; } = string.Empty;
		public double Evaluation { get; set; }
	}
}
