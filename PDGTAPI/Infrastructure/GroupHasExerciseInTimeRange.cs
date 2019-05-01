using System;
using System.Collections.Generic;

namespace PDGTAPI.Infrastructure
{
    public partial class GroupHasExerciseInTimeRange
    {
        public int GroupId { get; set; }
        public int ExerciseId { get; set; }
        public int TimeRangeId { get; set; }

        public Exercise Exercise { get; set; }
        public RandomisationGroup Group { get; set; }
        public TimeRange TimeRange { get; set; }
    }
}
