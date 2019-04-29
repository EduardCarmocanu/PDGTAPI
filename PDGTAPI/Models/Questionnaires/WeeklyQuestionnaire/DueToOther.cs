using System.ComponentModel.DataAnnotations;

namespace PDGTAPI.Models.Questionnaires.WeeklyQuestionnaire
{
	public class DueToOther
	{
		[Required]
		public bool HasTaken { get; set; }
		[StringLength(1024, ErrorMessage = "Reason for taking painkillers too long", MinimumLength = 2)]
		public string TakingReason { get; set; }
		[Range(1, 7)]
		public int DaysTaken { get; set; }
		public Painkiller[] Painkillers { get; set; }
	}
}