using System;
using System.Collections.Generic;

namespace PDGTAPI.Infrastructure
{
    public partial class Exercise
    {
        public Exercise()
        {
            GroupHasExerciseInTimeRange = new HashSet<GroupHasExerciseInTimeRange>();
            UserHasExerciseInTimeRange = new HashSet<UserHasExerciseInTimeRange>();
        }

        public int Id { get; set; }
        public string ExerciseName { get; set; }

        public ICollection<GroupHasExerciseInTimeRange> GroupHasExerciseInTimeRange { get; set; }
        public ICollection<UserHasExerciseInTimeRange> UserHasExerciseInTimeRange { get; set; }
    }
}
