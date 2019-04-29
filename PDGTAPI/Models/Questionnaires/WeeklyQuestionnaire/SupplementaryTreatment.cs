using System.ComponentModel.DataAnnotations;

namespace PDGTAPI.Models.Questionnaires.WeeklyQuestionnaire
{
	public class SupplementaryTreatment
	{
		[Required]
		public bool SeenDoctorDueToShoulder { get; set; }
		[Required]
		public bool ReceivedTreatment { get; set; }
		public string[] Treatments { get; set; }
	}
}