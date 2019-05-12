using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDGTAPI.DTOs;
using PDGTAPI.Helpers;
using PDGTAPI.Models;
using PDGTAPI.Services;

namespace PDGTAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Policy = "Patients")]
	public class WeekController : ControllerBase
	{
		private readonly IWeekService _weekService;
		private readonly IRedCapService _redCapService;

		public WeekController(IWeekService weekService, IRedCapService redCapService)
		{
			_weekService = weekService;
			_redCapService = redCapService;
		}

		[HttpGet]
		[Route("state")]
		public ActionResult State()
		{
			WeekStateDTO result = new WeekStateDTO();
			ServiceResult<WeekStateDTO> weekState = _weekService.GetState(User.Identity.Name);

			result = weekState.Content;

			return Ok(result);
		}

		[HttpPost]
		[Route("sessions/questionnaire")]
		public async Task<ActionResult<string>> PostSessionsQuestionnaire([FromBody] Session session)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			ServiceResult<string> result = await _redCapService.PostSessionQuestionnaireAsync(session, User.Identity.Name);

			if (result.Succeded)
			{
				return Ok(result.Content);
			}

			return Forbid(result.ErrorMessage);
		}
	}
} 