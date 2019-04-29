using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Models.Questionnaires.WeeklyQuestionnaire
{
	public class Painkillers
	{
		[Required]
		public DueToTraining DueToTraining { get; set; }
		[Required]
		public DueToOther DueToOther { get; set; }
	}
}
