using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PDGTAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Data
{
	public class ApplicationDataContext : IdentityDbContext<UserEntity, IdentityRole, string>
	{
		public ApplicationDataContext(DbContextOptions options) : base(options) { }
	}
}
