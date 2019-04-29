using Microsoft.AspNetCore.Identity;
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

		public DataSeeder(RoleManager<IdentityRole> roleManager)
		{
			_roleManager = roleManager;
		}

		public void Seed()
		{
			if (!_roleManager.Roles.Any())
			{
				_roleManager.CreateAsync(new IdentityRole("Administrator"));
				_roleManager.CreateAsync(new IdentityRole("Doctor"));
				_roleManager.CreateAsync(new IdentityRole("Patient"));
			}
		}
	}
}
