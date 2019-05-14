using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDGTAPI.Helpers;
using PDGTAPI.Models;
using PDGTAPI.Services;

namespace PDGTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
		private readonly IUsersService _usersService;

		public PatientsController(IUsersService usersService)
		{
			_usersService = usersService;
		}

		[HttpPost]
		[Route("register")]
		[Authorize(Policy = "Physiotherapists")]
		public async Task<IActionResult> RegisterPatientAsync([FromBody] PatientRegistration model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			ServiceResult<string> registrationResult = await _usersService.RegisterPatientAsync(model, User.Identity.Name);
			if (!registrationResult.Succeded)
				return BadRequest(registrationResult.ErrorMessage);

			return Ok();
		}
    }
}
