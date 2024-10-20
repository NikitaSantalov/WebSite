using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebSite.Models;

namespace WebSite.Repositoryes.Interfaces
{
	public interface IRepository<T> where T : Entity
	{
		Task<T?> Get(int id);
		Task<IEnumerable<T>> Where(Expression<Func<T, bool>> predicate);
		Task<IResult> Add(T item);
		Task<IResult> Update(T item);
		Task<IResult> Remove(int id);
	}
}
