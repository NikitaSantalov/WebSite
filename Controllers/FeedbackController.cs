using System.Collections.Generic;
using System.Security.Claims;
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
		public IResult GetAll(int productId)
		{
			var result = new List<Feedback>(_feedbackRepo.Where(f => f.ProductId == productId));

			if (result.Count == 0)
			{
				return Results.NotFound();
			}

			return Results.Ok(result);
		}

		[HttpPost]
		[Route("feedback")]
		[Authorize(Roles = "Customer")]
		public IResult AddFeedback([FromBody] Feedback feedback)
		{
			if (!_validationService.ValidateFeedback(feedback))
			{
				return Results.BadRequest();
			}

			feedback.CustomerId = GetCustomerId();

			return _feedbackRepo.Add(feedback);
		}

		[HttpPatch]
		[Route("feedback")]
		[Authorize(Roles = "Customer")]
		public IResult UpdateFeedback([FromBody] Feedback feedback)
		{
			if (!_validationService.ValidateFeedback(feedback))
			{
				return Results.BadRequest();
			}

			var feed = _feedbackRepo.Get(feedback.Id);

			if (feed != null && AuthorizeCustomer(feed.CustomerId))
			{
				return Results.Unauthorized();
			}

			return _feedbackRepo.Update(feedback);
		}

		[HttpDelete]
		[Route("feedback")]
		[Authorize(Roles = "Customer, Admin")]
		public IResult DeleteFeedback(int id)
		{
			var feedback = _feedbackRepo.Get(id);

			if (feedback != null && !AuthorizeCustomer(feedback.CustomerId))
			{
				return Results.Unauthorized();
			}

			return _feedbackRepo.Remove(id);
		}

		private bool AuthorizeCustomer(int id)
		{
			var helper = JwtHelper.GetJwt(HttpContext.Request.Headers.Authorization);
			string role = helper.GetValue(ClaimTypes.Role);
			int customerId = int.Parse(helper.GetValue("Id"));

			return role == "Admin" | role == "Customer" & customerId == id;
		}

		private int GetCustomerId()
		{
			var helper = JwtHelper.GetJwt(HttpContext.Request.Headers.Authorization);
			return int.Parse(helper.GetValue("Id"));
		}
	}
}
