namespace WebSite.Models
{
    public record CustomerProduct : Entity
    {
        public int CustomerId { get; set; } 
        public int ProductId { get; set; }
    }
}
