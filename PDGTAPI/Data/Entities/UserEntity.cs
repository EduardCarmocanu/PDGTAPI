using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Data.Entities
{
	public class UserEntity : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int? RedCapRecordId { get; set; }
		public char? RedCapGroup { get; set; }
	}
}
