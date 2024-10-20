using System.Threading.Tasks;
using WebSite.Models;
using WebSite.Models.Dto;

namespace WebSite.Services.Interfaces
{
	public interface IDtoService<T, D> where T : Entity where D: EntityDto
	{
		D ToDto(T model);
		Task<T?> FromDto(D dto);
	}
}
