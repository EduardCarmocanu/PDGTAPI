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
		[StringLength(1024, ErrorMessage = "Comment is too long", MinimumLength = 16)]
		public string SkippingReason { get; set; }
		[Required]
		public Set Sets { get; set; }
	}
}
