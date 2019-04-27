using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PDGTAPI.Models.Entities;
using PDGTAPI.DTOs;
using PDGTAPI.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PDGTAPI.Services
{
	public interface IUsersService
	{
		Task<string> AuthenticateAsync(UserLoginDTO model);
		Task<string> RegisterPatientAsync(PatientRegistrationDTO model);
		Task<string> RegisterDoctorAsync(DoctorRegistrationDTO model);
	}
	
	public class UsersService : IUsersService
	{
		private readonly UserManager<UserEntity> _userManager;
		private readonly SignInManager<UserEntity> _signInManager;
		private readonly IConfiguration _configuration;
		private readonly IRedCapService _redCapService;

		public UsersService
		(
			UserManager<UserEntity> userManager,
			SignInManager<UserEntity> signInManager,
			IConfiguration configuration,
			IRedCapService redCapService
		)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_configuration = configuration;
			_redCapService = redCapService;
		}

		public async Task<string> AuthenticateAsync(UserLoginDTO model)
		{
			if (model == null)
				throw new ArgumentNullException();

			var loginResult = await _signInManager.PasswordSignInAsync(
				model.Email,
				model.Password,
				isPersistent: false,
				lockoutOnFailure: false
			);

			if (!loginResult.Succeeded) return null;

			UserEntity user = await _userManager.FindByEmailAsync(model.Email);
			return GetToken(user);
		}

		public async Task<string> RegisterDoctorAsync(DoctorRegistrationDTO model)
		{
			if (model == null)
				throw new ArgumentNullException();

			UserEntity user = new UserEntity
			{
				UserName = model.Email,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName
			};
			var identityResult = await _userManager.CreateAsync(user, model.Password);

			if (identityResult.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, Roles.Doctor);
				return GetToken(user);
			}

			return null;
		}

		public async Task<string> RegisterPatientAsync(PatientRegistrationDTO model)
		{
			if (model == null)
				throw new ArgumentNullException();

			if (!_redCapService.RecordExists(model.RedCapRecordId))
				return null;

			char? PatientGroup = _redCapService.GetRecordGroup(model.RedCapRecordId);
			if (PatientGroup == null)
				return null;

			UserEntity user = new UserEntity
			{
				UserName = model.Email,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				RedCapRecordId = model.RedCapRecordId,
				RedCapGroup = PatientGroup
			};
			var identityResult = await _userManager.CreateAsync(user, model.Password);

			if (identityResult.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, Roles.Patient);
				return GetToken(user);
			}

			return null;
		}

		private string GetToken(UserEntity TokenUser)
		{
			var utcNow = DateTime.UtcNow;

			Claim[] claims = new Claim[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, TokenUser.Id),
				new Claim(JwtRegisteredClaimNames.UniqueName, TokenUser.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString())
			};

			SymmetricSecurityKey signingKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_configuration["Security:JWT:Key"])
			);
			SigningCredentials signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
			JwtSecurityToken jwt = new JwtSecurityToken(
				signingCredentials: signingCredentials,
				claims: claims,
				notBefore: utcNow,
				expires: utcNow.AddSeconds(_configuration.GetValue<int>("Security:JWT:LifeTime")),
				audience: _configuration["Security:JWT:Audience"],
				issuer: _configuration["Security:JWT:Issuer"]
			);

			return new JwtSecurityTokenHandler().WriteToken(jwt);
		}
	}


}
