using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Helpers
{
	public class ServiceResult<T>
	{
		public T Content { get; set; }
		public string ErrorMessage { get; set; }
		public bool Succeded { get; set; }
	}
}
