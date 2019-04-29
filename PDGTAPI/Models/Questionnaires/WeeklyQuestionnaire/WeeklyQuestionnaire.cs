using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Models.Questionnaires.WeeklyQuestionnaire
{
	public class WeeklyQuestionnaire
	{
		[Required]
		public int NumberOfSessions { get; set; }
		[StringLength(1024, ErrorMessage = "Reason for skipping too long", MinimumLength = 2)]
		public string ReasonForSkippingSessions { get; set; }
		[Required]
		public Pain Pain { get; set; }
		[Required]
		public Symptoms Symptoms { get; set; }
		[Required]
		public Painkillers Painkillers { get; set; }
		[Required]
		public SickLeave SickLeave { get; set; }
		[Required]
		public SupplementaryTreatment SupplementaryTreatment { get; set; }
		[Required]
		public SideEffects SideEffects{ get; set; }
	}
}
