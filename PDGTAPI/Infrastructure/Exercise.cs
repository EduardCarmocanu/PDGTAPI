using System;
using System.Collections.Generic;

namespace PDGTAPI.Infrastructure
{
    public partial class Exercise
    {
        public Exercise()
        {
            GroupHasExerciseInTimeRange = new HashSet<GroupHasExerciseInTimeRange>();
            UserHasExerciseWeightInTimeRange = new HashSet<UserHasExerciseWeightInTimeRange>();
        }

        public int Id { get; set; }
        public string ExerciseName { get; set; }
        public int GuideId { get; set; }

        public Guide Guide { get; set; }
        public ICollection<GroupHasExerciseInTimeRange> GroupHasExerciseInTimeRange { get; set; }
        public ICollection<UserHasExerciseWeightInTimeRange> UserHasExerciseWeightInTimeRange { get; set; }
    }
}
