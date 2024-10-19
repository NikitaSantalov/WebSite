
using Microsoft.EntityFrameworkCore;

namespace WebSite.Models
{
	public record Product : Entity
	{
		public int SellerId { get; set; }
		public string Category { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public int Size { get; set; }
		[Precision(18, 2)]
		public decimal Cost { get; set; }
		public int Quantity { get; set; }
		public double Discount { get; set; }
		public string Date { get; set; } = string.Empty;
	}
}
