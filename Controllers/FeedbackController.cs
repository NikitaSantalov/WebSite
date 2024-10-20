using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebSite.Helpers;
using WebSite.Models;
using WebSite.Repositoryes.Interfaces;
using WebSite.Services.Interfaces;

namespace WebSite.Controllers
{
	[Route("api")]
	[Controller]
	public class FeedbackController : Controller
	{
		private readonly IRepository<Feedback> _feedbackRepo;
		private readonly IValidationService _validationService;

		public FeedbackController(IRepository<Feedback> repository, IValidationService validationService)
		{
			_feedbackRepo = repository;
			_validationService = validationService;
		}

		[HttpGet]
		[Route("feedback")]
		public async Task<IResult> GetAll(int productId)
		{
			var result = new List<Feedback>(await _feedbackRepo.Where(f => f.ProductId == productId));

			if (result.Count == 0)
			{
				return Results.NotFound();
			}

			return Results.Ok(result);
		}

		[HttpPost]
		[Route("feedback")]
		[Authorize(Roles = "Customer")]
		public async Task<IResult> AddFeedback([FromBody] Feedback feedback)
		{
			if (!_validationService.ValidateFeedback(feedback))
			{
				return Results.BadRequest();
			}

			var helper = JwtHelper.GetJwt(HttpContext.Request.Headers.Authorization);

			if (helper == null)
			{
				return Results.BadRequest();
			}

			feedback.CustomerId = int.Parse(helper.GetValue("Id"));

			return await _feedbackRepo.Add(feedback);
		}

		[HttpPatch]
		[Route("feedback")]
		[Authorize(Roles = "Customer")]
		public async Task<IResult> UpdateFeedback([FromBody] Feedback feedback)
		{
			if (!_validationService.ValidateFeedback(feedback))
			{
				return Results.BadRequest();
			}

			var helper = JwtHelper.GetJwt(HttpContext.Request.Headers.Authorization);

			if (helper == null)
			{
				return Results.BadRequest();
			}

			int customerId = int.Parse(helper.GetValue("Id"));
			var feed = await _feedbackRepo.Get(feedback.Id);

			if (feed == null)
			{
				return Results.NotFound();
			}

			if (customerId != feed.CustomerId)
			{
				return Results.Unauthorized();
			}

			return await _feedbackRepo.Update(feedback);
		}

		[HttpDelete]
		[Route("feedback")]
		[Authorize(Roles = "Customer, Admin")]
		public async Task<IResult> DeleteFeedback(int id)
		{
			var helper = JwtHelper.GetJwt(HttpContext.Request.Headers.Authorization);

			if (helper == null)
			{
				return Results.BadRequest();
			}

			string role = helper.GetValue(ClaimTypes.Role);
			int customerId = 0;
			
			if (role == "Customer")
			{
				customerId = int.Parse(helper.GetValue("Id"));
			}
			
			var feedback = await _feedbackRepo.Get(id);

			if (feedback == null)
			{
				return Results.NotFound();
			}

			if (role == "Customer" & customerId != feedback.CustomerId)
			{
				return Results.Unauthorized();
			}

			return await _feedbackRepo.Remove(id);
		}
	}
}
