using System;
using System.Collections.Generic;

namespace PDGTAPI.Infrastructure
{
    public partial class UserHasExerciseWeightInTimeRange
    {
        public string UserId { get; set; }
        public int ExerciseId { get; set; }
        public int TimeRangeId { get; set; }
        public byte UserExerciseWeight { get; set; }

        public Exercise Exercise { get; set; }
        public TimeRange TimeRange { get; set; }
        public User User { get; set; }
    }
}
