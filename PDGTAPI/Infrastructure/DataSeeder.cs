using Microsoft.AspNetCore.Identity;
using PDGTAPI.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Infrastructure
{
	public class DataSeeder
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<User> _userManager;
		private readonly ApplicationDataContext _context;

		public DataSeeder(
			RoleManager<IdentityRole> roleManager, 
			UserManager<User> userManager, 
			ApplicationDataContext context
		)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_context = context;
		}

		public void Seed()
		{
			if (!_roleManager.Roles.Any())
			{
				_roleManager.CreateAsync(new IdentityRole(Roles.Administrator)).Wait();
				_roleManager.CreateAsync(new IdentityRole(Roles.Physiotherapist)).Wait();
				_roleManager.CreateAsync(new IdentityRole(Roles.Patient)).Wait();
			}

			if (!_context.RandomisationGroup.Any())
			{
				RandomisationGroup control = new RandomisationGroup
				{
					GroupName = Groups.Control
				};
				RandomisationGroup intervention = new RandomisationGroup
				{
					GroupName = Groups.Intervention
				};

				_context.RandomisationGroup.AddAsync(control).Wait();
				_context.RandomisationGroup.AddAsync(intervention).Wait();
				_context.SaveChangesAsync().Wait();
			}

			if (!_userManager.Users.Any())
			{
				var randomisationGroup = _context.RandomisationGroup.FirstOrDefault(
					x => x.GroupName == Groups.Intervention);

				DateTime baselineDate = new DateTime(
					DateTime.UtcNow.Year,
					DateTime.UtcNow.Month, 
					DateTime.UtcNow.Day, 
					0, // Hours
					0, // Minutes
					0  // Seconds
				);

				User patient = new User()
				{
					UserName = "patient@email.com",
					Email = "patient@email.com",
					RedCapRecordId = 1,
					RandomisationGroupID = randomisationGroup.Id,
					RedCapBaseline = baselineDate
				};
				IdentityResult patientResult = _userManager.CreateAsync(patient, "Password123!").Result;
				if (patientResult.Succeeded)
				{
					_userManager.AddToRoleAsync(patient, Roles.Patient).Wait();
				}

				User doctor = new User()
				{
					UserName = "physio@email.com",
					Email = "physio@email.com",
				};
				IdentityResult doctorResult = _userManager.CreateAsync(doctor, "Password123!").Result;
				if (doctorResult.Succeeded)
				{
					_userManager.AddToRoleAsync(doctor, Roles.Physiotherapist).Wait();
				}

				User admin = new User()
				{
					UserName = "admin@email.com",
					Email = "admin@email.com",
				};
				IdentityResult adminResult = _userManager.CreateAsync(admin, "Password123!").Result;
				if (adminResult.Succeeded)
				{
					_userManager.AddToRoleAsync(admin, Roles.Administrator).Wait();
				}
			}
		}
	}
}
