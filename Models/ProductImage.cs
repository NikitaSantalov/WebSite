namespace WebSite.Models
{
    public record ProductImage : Entity
    {
        public int ProductId { get; set; }
        public string ImagePath { get; set; } = string.Empty;
	}
}
