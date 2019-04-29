using System.ComponentModel.DataAnnotations;

namespace PDGTAPI.Models.Questionnaires.WeeklyQuestionnaire
{
	public class SideEffects
	{
		[Required]
		public bool Experienced { get; set; }
		public string[] List { get; set; }
		[Required]
		public bool AcceptableToContinueTraining { get; set; }
	}
}