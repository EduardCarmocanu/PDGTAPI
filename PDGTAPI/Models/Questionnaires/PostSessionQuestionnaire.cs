using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Models.Questionnaires
{
	public class PostSessionQuestionnaire
	{
		public int PainAfterSession { get; set; }
		public bool PainkillersAfterSession { get; set; }
		public string PainkillerType { get; set; }
		public int PainkillersQuantity { get; set; }
		public bool TiredAfterSession { get; set; }
		public string Comments { get; set; }
	}
}
