using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Models.Set
{
	public class SetIntervention
	{
		[Required]
		public int Repetitions { get; set; }
		[Required]
		public int Weight { get; set; }
	}
}
