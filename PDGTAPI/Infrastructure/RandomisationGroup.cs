using System;
using System.Collections.Generic;

namespace PDGTAPI.Infrastructure
{
    public partial class RandomisationGroup
    {
        public RandomisationGroup()
        {
            Users = new HashSet<User>();
			TimeRanges = new HashSet<TimeRange>();
		}

        public int Id { get; set; }
        public string GroupName { get; set; }

        public ICollection<User> Users { get; set; }
		public ICollection<TimeRange> TimeRanges { get; set; }
	}
}
