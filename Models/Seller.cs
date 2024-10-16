﻿
namespace WebSite.Models
{
	public record Seller : Entity
	{
		public string Password { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
	}
}
