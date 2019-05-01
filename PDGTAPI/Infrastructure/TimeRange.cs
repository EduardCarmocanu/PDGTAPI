using System;
using System.Collections.Generic;

namespace PDGTAPI.Infrastructure
{
    public partial class TimeRange
    {
        public TimeRange()
        {
            GroupHasExerciseInTimeRange = new HashSet<GroupHasExerciseInTimeRange>();
            UserHasExerciseWeightInTimeRange = new HashSet<UserHasExerciseWeightInTimeRange>();
        }

        public int Id { get; set; }
        public byte StartWeek { get; set; }
        public byte EndWeek { get; set; }
        public byte SetsAmount { get; set; }
        public byte RepsAmount { get; set; }

        public ICollection<GroupHasExerciseInTimeRange> GroupHasExerciseInTimeRange { get; set; }
        public ICollection<UserHasExerciseWeightInTimeRange> UserHasExerciseWeightInTimeRange { get; set; }
    }
}
