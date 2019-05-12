using System;
using System.Collections.Generic;

namespace PDGTAPI.Infrastructure
{
    public partial class Exercise
    {
        public Exercise()
        {
            TimeRangeHasExercises = new HashSet<TimeRangeHasExercise>();
        }

        public int Id { get; set; }
        public string ExerciseName { get; set; }

        public ICollection<TimeRangeHasExercise> TimeRangeHasExercises { get; set; }
    }
}
