namespace WebSite.Models.Dto
{
	public class CustomerDto : EntityDto
	{
		public string Email { get; set; } = string.Empty;
		public string PaymentType { get; set; } = string.Empty;
		public string DeliveryType { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
	}
}
