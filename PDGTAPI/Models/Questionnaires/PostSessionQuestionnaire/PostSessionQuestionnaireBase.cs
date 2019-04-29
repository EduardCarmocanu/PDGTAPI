using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Models.Questionnaires.PostSessionQuestionnaire
{
	public abstract class PostSessionQuestionnaireBase
	{
		[Required]
		[Range(0, 10)]
		public int Pain { get; set; }

		[Required]
		public bool TakenPainkillers { get; set; }

		public Painkiller[] Painkillers { get; set; }

		[Required]
		public bool TrainingSideEffects { get; set; }

		[Required]
		[StringLength(1024, ErrorMessage = "Comment is too long", MinimumLength = 16)]
		public string TrainingSideEffectsDescription { get; set; }

		[StringLength(1024, ErrorMessage = "Comment is tool long", MinimumLength = 16)]
		public string Comments { get; set; }

	}
}
