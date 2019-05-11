using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDGTAPI.DTOs;
using PDGTAPI.Helpers;
//using PDGTAPI.Infrastructure;
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
		public ActionResult<WeekStateDTO> State()
		{
			WeekStateDTO result = new WeekStateDTO();
			ServiceResult<WeekStateDTO> weekState = _weekService.GetState(User.Identity.Name);

			if (weekState.Succeded)
			{
				result = weekState.Content;
			}

			return Ok(result);
		}

		[HttpPost]
		[Route("sessions/questionnaire")]
		public ActionResult<SessionQuestionnaireDTO> SessionsQuestionnaire()
		{
			return Ok("SQS");
		}

			return await _redCapService.PostSessionQuestionnaireAsync(session, User.Identity.Name);
		}
	}
} 