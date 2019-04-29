using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Infrastructure.Entities
{
	public class User : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int? RedCapRecordId { get; set; }
		public char? RedCapGroup { get; set; }
		public DateTime RedCapBaseline { get; set; }
	}
}
