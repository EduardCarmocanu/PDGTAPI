using PDGTAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.DTOs
{
	public class WeekStateDTO
	{
		public List<ExerciseDTO> Exercises { get; set; }
		public bool AvailableWeeklyQuestionnaire { get; set; }
		public bool SessionsFinished { get; set; }
		public int WeekNumber { get; set; }
	}
}
