using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PDGTAPI.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Infrastructure
{
	public class ApplicationDataContext : IdentityDbContext<User, IdentityRole, string>
	{
		public ApplicationDataContext(DbContextOptions options) : base(options) { }	
	}
}
