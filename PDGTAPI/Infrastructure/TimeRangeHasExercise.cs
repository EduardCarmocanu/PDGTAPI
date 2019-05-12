using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Infrastructure
{
	public class TimeRangeHasExercise
	{
		public int ExerciseId { get; set; }
		public int TimeRangeId { get; set; }

		public Exercise Exercise { get; set; }
		public TimeRange TimeRange { get; set; }
	}
}
