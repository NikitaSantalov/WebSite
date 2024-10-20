using WebSite.Models;
using WebSite.Models.Dto;
using WebSite.Repositoryes.Interfaces;
using WebSite.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Services.Dto
{
	public class SellerDtoService : IDtoService<Seller, SellerDto>
	{
		private IRepository<Seller> _sellerRepo;
		public SellerDtoService(IRepository<Seller> sellerRepo)
		{
			_sellerRepo = sellerRepo;
		}

		public async Task<Seller?> FromDto(SellerDto dto)
		{
			var sellers = await _sellerRepo.Where(s => s.Name == dto.Name & s.Email == dto.Email & s.Phone == dto.Phone);
			return sellers.FirstOrDefault();
		}

		public SellerDto ToDto(Seller seller)
		{
			SellerDto dto = new SellerDto();
			dto.Id = seller.Id;
			dto.Name = seller.Name;
			dto.Email = seller.Email;
			dto.Phone = seller.Phone;

			return dto;
		}
	}
}
