using System.Linq;
using System.Threading.Tasks;
using WebSite.Models;
using WebSite.Models.Dto;
using WebSite.Repositoryes.Interfaces;
using WebSite.Services.Interfaces;

namespace WebSite.Services.Dto
{
	public class CustomerDtoService : IDtoService<Customer, CustomerDto>
	{
		private IRepository<Customer> _customerRepo;

		public CustomerDtoService(IRepository<Customer> customerRepo)
		{
			_customerRepo = customerRepo;
		}

		public async Task<Customer?> FromDto(CustomerDto dto)
		{
			var customers = await _customerRepo.Where(c => c.Email == dto.Email & c.Name == dto.Name & c.LastName == dto.LastName);
			return customers.FirstOrDefault();
		}

		public CustomerDto ToDto(Customer model)
		{
			var dto = new CustomerDto();
			dto.Id = model.Id;
			dto.Name = model.Name;
			dto.LastName = model.LastName;
			dto.Email = model.Email;
			dto.Phone = model.Phone;

			return dto;
		}
	}
}
