using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Models.Questionnaires
{
	public class PreSessionQuestionnaire
	{
		public int PainBeforeSession { get; set; }
		public bool PainkillersBeforeSession { get; set; }
		public string PainkillerType { get; set; }
		public int PainkillersQuantity { get; set; }
		public bool TrainingSideEffects { get; set; }
		public string TrainingSideEffectsDescription { get; set; }
	}
}
