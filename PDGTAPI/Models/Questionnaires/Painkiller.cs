using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Models.Questionnaires
{
	public class Painkiller
	{
		[Required]
		[StringLength(64, ErrorMessage = "Painkiller name must be between 2 and 64 character", MinimumLength = 2)]
		public string Type { get; set; }
		[Required]
		public int Amount { get; set; }
	}
}
