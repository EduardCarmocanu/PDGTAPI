using System;
using System.Collections.Generic;

namespace PDGTAPI.Infrastructure
{
    public partial class Session
    {
        public int Id { get; set; }
		public string UserId { get; set; }
		public DateTime CompletionTime { get; set; }

		public User User { get; set; }
	}
}
