using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Models.Questionnaires
{
	public class PostSessionQuestionnaire
	{
		[Required]
		[Range(0, 10)]
		public int Pain { get; set; }

		[Required]
		public bool TakenPainkillers { get; set; }

		public List<Painkiller> Painkillers { get; set; }

		[Required]
		public bool TrainingSideEffects { get; set; }

		[Required]
		[StringLength(1024, ErrorMessage = "Side effects description must be between 16 and 1024 chracters long", MinimumLength = 16)]
		public string TrainingSideEffectsDescription { get; set; }

		[StringLength(1024, ErrorMessage = "Comment description must be between 16 and 1024 chracters long", MinimumLength = 16)]
		public string Comments { get; set; }

		[Required]
		public bool Tired { get; set; }
	}
}
