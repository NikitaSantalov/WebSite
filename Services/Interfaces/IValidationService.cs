using WebSite.Models.Dto;
using WebSite.Models;

namespace WebSite.Services.Interfaces
{
	public interface IValidationService
	{
		public bool ValidateFeedback(Feedback feedback);

		public bool ValidateProduct(Product product);

		public bool ValidateUser(UserDto user);

		public bool ValidateSeller(Seller seller);

		public bool ValidateCustomer(Customer customer);
	}
}
