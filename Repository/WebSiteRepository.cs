using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebSite.Models;
using WebSite.Repositoryes.Interfaces;

namespace WebSite.Repositoryes
{
	public class WebSiteRepository<T> : IRepository<T> where T : Entity
	{
		private WebSiteContext _context;

		public WebSiteRepository(WebSiteContext context)
		{
			_context = context;
		}

		public T? Get(int id)
		{
			return _context.Set<T>().AsNoTracking().Where(i => i.Id == id).FirstOrDefault();
		}

		public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
		{
			return _context.Set<T>().AsNoTracking().Where(predicate);
		}

		public IResult Add(T item)
		{
			_context.Set<T>().Add(item);
			_context.SaveChanges();

			return Results.Ok();
		}


		public IResult Update(T item)
		{
			var res = _context.Set<T>().AsNoTracking().Where(i => i.Id == item.Id).FirstOrDefault();

			if (res == null)
			{
				return Results.NotFound();
			}

			_context.Set<T>().Update(item);
			_context.SaveChanges();

			return Results.Ok();
		}

		public IResult Remove(int id)
		{
			var item = _context.Set<T>().Where(i => i.Id == id).FirstOrDefault();

			if (item == null)
			{
				return Results.NotFound();
			}

			_context.Set<T>().Remove(item);
			_context.SaveChanges();

			return Results.Ok();
		}
	}
}
