using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Models.Questionnaires.WeeklyQuestionnaire
{
	public class SickLeave
	{
		[Required]
		public bool Taken { get; set; }
		[Range(1, 7)]
		public int Days { get; set; }
	}
}
