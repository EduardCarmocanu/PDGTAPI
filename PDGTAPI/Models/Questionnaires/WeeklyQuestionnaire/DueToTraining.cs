using System.ComponentModel.DataAnnotations;
using PDGTAPI.Models.Questionnaires;

namespace PDGTAPI.Models.Questionnaires.WeeklyQuestionnaire
{
	public class DueToTraining
	{
		[Required]
		public bool HasTaken { get; set; }
		[Range(1, 7)]
		public int DaysTaken { get; set; }
		public Painkiller[] Painkillers { get; set; }
	}
}