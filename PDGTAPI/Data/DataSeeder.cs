using Microsoft.AspNetCore.Identity;
using PDGTAPI.Data.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Data
{
	public class DataSeeder
	{
		private readonly RoleManager<IdentityRole> _roleManager;

		public DataSeeder(
			RoleManager<IdentityRole> roleManager
		)
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
