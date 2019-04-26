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
using PDGTAPI.Data.Entities;
using PDGTAPI.DTOs;

namespace PDGTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	[Produces("application/json")]
	[AllowAnonymous]
    public class AccountsController : ControllerBase
    {
		private readonly UserManager<UserEntity> _userManager;
		private readonly SignInManager<UserEntity> _signInManager;
		private readonly IConfiguration _configuration;
		private readonly ILogger<AccountsController> _logger;

		public AccountsController
		(
			UserManager<UserEntity> userManager,
			SignInManager<UserEntity> signInManager,
			IConfiguration configuration,
			ILogger<AccountsController> logger
		)
		{
			this._userManager = userManager;
			this._signInManager = signInManager;
			this._configuration = configuration;
			this._logger = logger;
		}

		[HttpPost]
		[Route("authenticate")]
		public async Task<IActionResult> Authenticate([FromBody] UserLoginDTO model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var loginResult = await _signInManager.PasswordSignInAsync(
				model.Email,
				model.Password,
				isPersistent: false,
				lockoutOnFailure: false
			);

			if (!loginResult.Succeeded) return BadRequest();

			UserEntity user = await _userManager.FindByEmailAsync(model.Email);
			return Ok(GetToken(user));
		}

		[HttpPost]
		[Route("register")]	
		public async Task<IActionResult> Register(UserRegistrationDTO model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			UserEntity user = new UserEntity
			{
				UserName = model.Email,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				RedCapGroup = model.RedCapGroup,
				RedCapRecordId = model.RedCapRecordId
			};

			var identityResult = await this._userManager.CreateAsync(user, model.Password);

			if (identityResult.Succeeded) return Ok(GetToken(user));

			return BadRequest(identityResult);
		}

		private string GetToken(UserEntity user)
		{
			var utcNow = DateTime.UtcNow;

			Claim[] claims = new Claim[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id),
				new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString())
			};

			SymmetricSecurityKey signingKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(this._configuration["Security:JWT:Key"])
			);
			SigningCredentials signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
			JwtSecurityToken jwt = new JwtSecurityToken(
				signingCredentials: signingCredentials,
				claims: claims,
				notBefore: utcNow,
				expires: utcNow.AddSeconds(this._configuration.GetValue<int>("Security:JWT:LifeTime")),
				audience: this._configuration["Security:JWT:Audience"],
				issuer: this._configuration["Security:JWT:Issuer"]
			);

			return new JwtSecurityTokenHandler().WriteToken(jwt);
		}
	}
}