using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

		public async Task<T?> Get(int id)
		{
			return await _context.Set<T>().AsNoTracking().Where(i => i.Id == id).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<T>> Where(Expression<Func<T, bool>> predicate)
		{
			return await _context.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
		}

		public async Task<IResult> Add(T item)
		{
			_context.Set<T>().Add(item);
			await _context.SaveChangesAsync();

			return Results.Ok();
		}


		public async Task<IResult> Update(T item)
		{
			var res = _context.Set<T>().AsNoTracking().Where(i => i.Id == item.Id).FirstOrDefault();

			if (res == null)
			{
				return Results.NotFound();
			}

			_context.Set<T>().Update(item);
			await _context.SaveChangesAsync();

			return Results.Ok();
		}

		public async Task<IResult> Remove(int id)
		{
			var item = _context.Set<T>().Where(i => i.Id == id).FirstOrDefault();

			if (item == null)
			{
				return Results.NotFound();
			}

			_context.Set<T>().Remove(item);
			await _context.SaveChangesAsync();

			return Results.Ok();
		}
	}
}
