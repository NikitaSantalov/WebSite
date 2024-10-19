using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using WebSite.Models;

namespace WebSite.Repositoryes.Interfaces
{
	public interface IRepository<T> where T : Entity
	{
		T? Get(int id);
		IEnumerable<T> Where(Expression<Func<T, bool>> predicate);
		IResult Add(T item);
		IResult Update(T item);
		IResult Remove(int id);
	}
}
