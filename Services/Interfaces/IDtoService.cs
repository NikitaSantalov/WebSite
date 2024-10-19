using WebSite.Models;
using WebSite.Models.Dto;

namespace WebSite.Services.Interfaces
{
	public interface IDtoService<T, D> where T : Entity where D: EntityDto
	{
		D ToDto(T model);
		T? FromDto(D dto);
	}
}
