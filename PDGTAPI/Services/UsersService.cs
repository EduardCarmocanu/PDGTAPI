using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PDGTAPI.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using PDGTAPI.Models;
using PDGTAPI.Infrastructure;

namespace PDGTAPI.Services
{
	public interface IUsersService
	{
		Task<ServiceResult<string>> AuthenticateAsync(UserLogin model);
		Task<ServiceResult<string>> RegisterPatientAsync(PatientRegistration model, string physioUserName);
		Task<ServiceResult<string>> RegisterPhysiotherapistAsync(DoctorRegistration model);
	}
	
	public class UsersService : IUsersService
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IConfiguration _configuration;
		private readonly IRedCapService _redCapService;
		private readonly ApplicationDataContext _context;

		public UsersService
	(
			UserManager<User> userManager,
			SignInManager<User> signInManager,
			IConfiguration configuration,
			IRedCapService redCapService,
			ApplicationDataContext context
		)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_configuration = configuration;
			_redCapService = redCapService;
			_context = context;
		}

		public async Task<ServiceResult<string>> AuthenticateAsync(UserLogin model)
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

			User user = await _userManager.FindByEmailAsync(model.Email);
			result.Content = GetToken(user);
			result.Succeded = true;

			return result;
		}

		public async Task<ServiceResult<string>> RegisterPhysiotherapistAsync(DoctorRegistration model)
		{
			if (model == null)
				throw new ArgumentNullException();

			ServiceResult<string> result = new ServiceResult<string>();

			User user = new User
			{
				UserName = model.Email,
				Email = model.Email,
			};

			var identityResult = await _userManager.CreateAsync(user, model.Password);
			if (identityResult.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, Roles.Physiotherapist);
				result.Content = GetToken(user);
				result.Succeded = true;

				return result;
			}

			foreach (var error in identityResult.Errors)
			{
				result.ErrorMessage = error.Description + "\n\r";
			}
			return result;
		}

		public async Task<ServiceResult<string>> RegisterPatientAsync(PatientRegistration model, string physioUserName)
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

			ServiceResult<UserInfo> redCapRecordInformationResult = await _redCapService.GetRecordInformationAsync(model.RedCapRecordId);
			if (!redCapRecordInformationResult.Succeded)
			{
				result.ErrorMessage = redCapRecordInformationResult.ErrorMessage;
				return result;
			}

			int randomisationGroupId = _context.RandomisationGroup.SingleOrDefault(
				x => x.GroupName == redCapRecordInformationResult.Content.RandomisationGroup
			).Id;

			string doctorId = _context.Users.FirstOrDefault(x => x.UserName == physioUserName).Id;

			User user = new User
			{
				UserName = model.Email,
				Email = model.Email,
				RedCapRecordId = redCapRecordInformationResult.Content.RecordId,
				RandomisationGroupID = randomisationGroupId,
				RedCapBaseline = redCapRecordInformationResult.Content.BaselineDate,
				DoctorId = doctorId
			};

			var identityResult = await _userManager.CreateAsync(user, model.Password);
			if (identityResult.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, Roles.Patient);
				result.Succeded = true;
				result.Content = GetToken(user);

				return result;
			}

			foreach (var error in identityResult.Errors)
			{
				result.ErrorMessage = error.Description + "\n\r";
			}
			return result;
		}

		private string GetToken(User TokenUser)
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
