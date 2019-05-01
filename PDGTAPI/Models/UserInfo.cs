using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Models
{
	public class UserInfo
	{
		public int RecordId { get; set; }
		public DateTime BaselineDate { get; set; }
		public char RandomisationGroup { get; set; }
	}
}
