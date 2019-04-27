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
using PDGTAPI.Models.Entities;
using PDGTAPI.DTOs;
using PDGTAPI.Services;

namespace PDGTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	[Produces("application/json")]
	[AllowAnonymous]
    public class AccountsController : ControllerBase
    {
		private readonly IUsersService _usersService;

		public AccountsController(IUsersService usersService)
		{
			_usersService = usersService;
		}

		[HttpPost]
		[Route("authenticate")]
		public async Task<IActionResult> AuthenticateAsync([FromBody] UserLoginDTO model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			string AuthenticationResult = await _usersService.AuthenticateAsync(model);
			if (AuthenticationResult == null)
				return BadRequest("Wrong Username or Password");

			return Ok(AuthenticationResult);
		}

		[HttpPost]
		[Route("registerdoctor")]
		public async Task<IActionResult> RegisterDoctorAsync([FromBody] DoctorRegistrationDTO model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			string DoctorRegistrationResult = await _usersService.RegisterDoctorAsync(model);
			if (DoctorRegistrationResult == null)
				return BadRequest("Could not register user");

			return Ok(DoctorRegistrationResult);
		}

		[HttpPost]
		[Route("registerpatient")]
		public async Task<IActionResult> RegisterPatientAsync([FromBody] PatientRegistrationDTO model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			string PatientRegistrationResult = await _usersService.RegisterPatientAsync(model);
			if (PatientRegistrationResult == null)
				return BadRequest("Could not register user");

			return Ok(PatientRegistrationResult);
		}
	}
}