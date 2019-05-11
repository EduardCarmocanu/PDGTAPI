using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDGTAPI.Infrastructure
{
    public partial class TimeRange
    {
        public TimeRange()
        {
			TimeRangeHasExercises = new HashSet<TimeRangeHasExercise>();
		}

        public int Id { get; set; }
        public byte StartWeek { get; set; }
        public byte EndWeek { get; set; }
        public byte SetsAmount { get; set; }
        public byte RepsAmount { get; set; }
		[StringLength(2)]
		public string RedCapIdentifier { get; set; }
		public int RandomisationGroupID { get; set; }
		public string RandomisationGroup { get; set; }

		public ICollection<TimeRangeHasExercise> TimeRangeHasExercises { get; set; }
	}
}
