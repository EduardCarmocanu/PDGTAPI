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
    [Route("api/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
		private readonly IUsersService _usersService;

		public AuthenticationController(IUsersService usersService)
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
				return Unauthorized();

			return Ok(authenticationResult.Content);
		}
	}
}