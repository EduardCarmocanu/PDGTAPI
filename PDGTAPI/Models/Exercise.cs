using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Models
{
	public class Exercise <TSet>
	{
		[Required]
		public bool IsSkipped { get; set; }
		public string SkippingReason { get; set; }
		[Required]
		public List<TSet> Sets { get; set; }
	}
}
