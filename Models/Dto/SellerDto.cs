﻿namespace WebSite.Models.Dto
{
	public class SellerDto : EntityDto
	{
		public int Id { get; set; }
		public string Email { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
	}
}
