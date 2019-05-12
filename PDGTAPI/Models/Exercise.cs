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
		public bool IsSkipped { get; set; }
		[StringLength(1024, ErrorMessage = "Reason must be between 16 and 1024 character", MinimumLength = 16)]
		public string SkippingReason { get; set; }
		[Required]
		public List<Set> Sets { get; set; }
	}
}
