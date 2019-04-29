using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Models.Questionnaires.WeeklyQuestionnaire
{
	public class Pain
	{
		[Required]
		[Range(0, 10)]
		public int Lowest { get; set; }
		[Required]
		[Range(0, 10)]
		public int Highest { get; set; }
		[Required]
		[Range(0, 10)]
		public int Average { get; set; }
	}
}
