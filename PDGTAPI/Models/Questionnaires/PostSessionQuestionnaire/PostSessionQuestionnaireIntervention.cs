using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Models.Questionnaires.PostSessionQuestionnaire
{
	public class PostSessionQuestionnaireIntervention : PostSessionQuestionnaireBase
	{
		[Required]
		public bool Tired { get; set; }
	}
}
