using PDGTAPI.Models.Questionnaires;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Models
{
	public class Session <TPostSessionQuestionnaire, TSet>
	{
		[Required]
		public Exercise<TSet>[] Exercises { get; set; }
		[Required]
		public PreSessionQuestionnaire PreQuestionaire { get; set; }
		[Required]
		public TPostSessionQuestionnaire PostQuestionnaire { get; set; }
	}
}
