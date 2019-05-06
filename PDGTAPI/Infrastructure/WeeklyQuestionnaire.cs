using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Infrastructure
{
	public class WeeklyQuestionnaire
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public bool Completed { get; set; }
		public DateTime CreationTime { get; set; }

		public User User { get; set; }
	}
}
