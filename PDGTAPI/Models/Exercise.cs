using PDGTAPI.Models.Questionnaires;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Models
{
	public class Exercise
	{
		[Required]
		public Set[] Sets { get; set; }
		[Required]
		public ExerciseQuestionnaire ExerciseQuestionnaire { get; set; }
		[Required]
		public bool IsSkipped { get; set; }
		[Required]
		public string SkippingReason { get; set; }
	}
}
