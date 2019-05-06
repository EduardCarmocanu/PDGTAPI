using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PDGTAPI.Services;
using PDGTAPI.Models;
using PDGTAPI.Helpers;

namespace PDGTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	[Produces("application/json")]
	[Authorize(Policy = "Doctors")]
    public class AccountsController : ControllerBase
    {
		private readonly IUsersService _usersService;

		public AccountsController(IUsersService usersService)
		{
			_usersService = usersService;
		}

		[HttpPost]
		[Route("authenticate")]
		[AllowAnonymous]
		public async Task<IActionResult> AuthenticateAsync([FromBody] UserLogin model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			ServiceResult<string> authenticationResult = await _usersService.AuthenticateAsync(model);
			if (!authenticationResult.Succeded)
				return BadRequest(authenticationResult.ErrorMessage);

			return Ok(authenticationResult);
		}

		[HttpPost]
		[Route("register/doctor")]
		[Authorize(Policy = "Administrators")]
		public async Task<IActionResult> RegisterDoctorAsync([FromBody] DoctorRegistration model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			ServiceResult<string> registrationResult = await _usersService.RegisterDoctorAsync(model);
			if (!registrationResult.Succeded)
				return BadRequest(registrationResult.ErrorMessage);

			return Ok(registrationResult);
		}

		[HttpPost]
		[Route("register/patient")]
		[Authorize(Policy = "Doctors")]
		public async Task<IActionResult> RegisterPatientAsync([FromBody] PatientRegistration model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			ServiceResult<string> registrationResult = await _usersService.RegisterPatientAsync(model);
			if (!registrationResult.Succeded)
				return BadRequest(registrationResult.ErrorMessage);

			return Ok(registrationResult);
		}
	}
}