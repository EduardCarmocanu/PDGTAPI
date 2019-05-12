using PDGTAPI.Models.Questionnaires;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Models
{
	public class Session
	{
		[Required]
		public List<Exercise> Exercises { get; set; }
		[Required]
		public PreSessionQuestionnaire PreQuestionnaire { get; set; }
		[Required]
		public PostSessionQuestionnaire PostQuestionnaire { get; set; }
	}
}
