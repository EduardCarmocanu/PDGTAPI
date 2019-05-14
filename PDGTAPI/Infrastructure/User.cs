using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Infrastructure
{
	public class User : IdentityUser
	{
		public User()
		{
			InverseDoctor = new HashSet<User>();
			Sessions = new HashSet<Session>();
		}
		public int? RedCapRecordId { get; set; }
		public int? RandomisationGroupID { get; set; }
		public DateTime? RedCapBaseline { get; set; }
		public string DoctorId { get; set; }

		public User Doctor { get; set; }
		public RandomisationGroup RandomisationGroup { get; set; }
		public ICollection<User> InverseDoctor { get; set; }
		public ICollection<Session> Sessions { get; set; }
	}
}
