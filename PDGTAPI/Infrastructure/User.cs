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
			GroupHasExerciseInTimeRange = new HashSet<GroupHasExerciseInTimeRange>();
			UserHasExerciseWeightInTimeRange = new HashSet<UserHasExerciseWeightInTimeRange>();
		}

		public int? RedCapRecordId { get; set; }
		public DateTime? RedCapBaseline { get; set; }
		public int? RandomisationGroupID { get; set; }

		public ICollection<GroupHasExerciseInTimeRange> GroupHasExerciseInTimeRange { get; set; }
		public ICollection<UserHasExerciseWeightInTimeRange> UserHasExerciseWeightInTimeRange { get; set; }
	}
}
