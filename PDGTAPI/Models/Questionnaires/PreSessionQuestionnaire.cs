using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Models.Questionnaires
{
	public class PreSessionQuestionnaire
	{
		[Required]
		[Range(0, 10)]
		public int Pain { get; set; }
		[Required]
		public bool TakenPainkillers { get; set; }
		public Painkiller[] Painkillers { get; set; }

	}
}
