using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PDGTAPI.Models.Entities;
using PDGTAPI.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using PDGTAPI.Models;

namespace PDGTAPI.Services
{
	public interface IUsersService
	{
		Task<ServiceResult<string>> AuthenticateAsync(UserLoginModel model);
		Task<ServiceResult<string>> RegisterPatientAsync(PatientRegistrationModel model);
		Task<ServiceResult<string>> RegisterDoctorAsync(DoctorRegistrationModel model);
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

		public async Task<ServiceResult<string>> AuthenticateAsync(UserLoginModel model)
		{
			if (model == null)
				throw new ArgumentNullException();

			ServiceResult<string> result = new ServiceResult<string>();

			var loginResult = await _signInManager.PasswordSignInAsync(
				model.Email,
				model.Password,
				isPersistent: false,
				lockoutOnFailure: false
			);

			if (!loginResult.Succeeded)
			{
				result.ErrorMessage = "Could not Authenticate user";
				return result;
			}

			UserEntity user = await _userManager.FindByEmailAsync(model.Email);
			result.Content = GetToken(user);
			result.Succeded = true;

			return result;
		}

		public async Task<ServiceResult<string>> RegisterDoctorAsync(DoctorRegistrationModel model)
		{
			if (model == null)
				throw new ArgumentNullException();

			ServiceResult<string> result = new ServiceResult<string>();

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
				result.Content = GetToken(user);
				result.Succeded = true;

				return result;
			}

			result.ErrorMessage = "Could not register Doctor";
			return result;
		}

		public async Task<ServiceResult<string>> RegisterPatientAsync(PatientRegistrationModel model)
		{
			if (model == null)
				throw new ArgumentNullException();

			ServiceResult<string> result = new ServiceResult<string>();

			if (_userManager.Users.Any(x => x.RedCapRecordId == model.RedCapRecordId))
			{
				result.ErrorMessage = 
					"A patient with RedCapRecordId: " 
					+ model.RedCapRecordId.ToString() 
					+ ", Already exists in the databse";
				return result;
			}

			ServiceResult<bool> recordExistsResult = _redCapService.RecordExists(model.RedCapRecordId);
			if (!recordExistsResult.Succeded)
			{
				result.ErrorMessage = recordExistsResult.ErrorMessage;
				return result;
			}
			if (!recordExistsResult.Content)
			{
				result.ErrorMessage = "Could not find patient record in RedCap database";
				return result;
			}

			ServiceResult<char> patientGroupResult = _redCapService.GetRecordGroup(model.RedCapRecordId);
			if (!patientGroupResult.Succeded)
			{
				result.ErrorMessage = patientGroupResult.ErrorMessage;
				return result;
			}

			UserEntity user = new UserEntity
			{
				UserName = model.Email,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				RedCapRecordId = model.RedCapRecordId,
				RedCapGroup = patientGroupResult.Content
			};

			var identityResult = await _userManager.CreateAsync(user, model.Password);
			if (identityResult.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, Roles.Patient);
				result.Succeded = true;
				result.Content = GetToken(user);

				return result;
			}

			result.ErrorMessage = "Could not register patient";
			return result;
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
