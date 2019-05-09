using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDGTAPI.Infrastructure
{
    public partial class TimeRange
    {
        public TimeRange()
        {
            GroupHasExerciseInTimeRange = new HashSet<GroupHasExerciseInTimeRange>();
            UserHasExerciseInTimeRange = new HashSet<UserHasExerciseInTimeRange>();
        }

        public int Id { get; set; }
        public byte StartWeek { get; set; }
        public byte EndWeek { get; set; }
        public byte SetsAmount { get; set; }
        public byte RepsAmount { get; set; }
		[StringLength(2)]
		public string RedCapIdentifier { get; set; }

        public ICollection<GroupHasExerciseInTimeRange> GroupHasExerciseInTimeRange { get; set; }
        public ICollection<UserHasExerciseInTimeRange> UserHasExerciseInTimeRange { get; set; }
    }
}
