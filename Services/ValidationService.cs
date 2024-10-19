using WebSite.Models;
using WebSite.Models.Dto;
using WebSite.Services.Interfaces;

namespace WebSite.Services
{
    public class ValidationService : IValidationService
    {
        public bool ValidateFeedback(Feedback feedback)
        {
            return feedback != null && feedback.Text != "" & 1 <= feedback.Evaluation & feedback.Evaluation <= 5;
        }

        public bool ValidateProduct(Product product)
        {
            return product != null && product.Name != "" & product.Category != "" & product.Cost > 0;
        }

        public bool ValidateUser(UserDto user)
        {
            return user != null && user.Email != "" & user.Password != "";
        }

        public bool ValidateSeller(Seller seller)
        {
            return seller != null && seller.Name != "" & seller.Email != "" & seller.Password != "" & seller.Phone != "";
        }

        public bool ValidateCustomer(Customer customer)
        {
            return customer != null && customer.Email != "" & customer.Password != "" &
                customer.Phone != "" & customer.Name != "" & customer.LastName != "";
        }
    }
}
