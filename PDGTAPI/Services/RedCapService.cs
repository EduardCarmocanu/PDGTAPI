using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Services
{
	public interface IRedCapService
	{
		bool CheckRecordExists(int RecordId);
		DateTime GetRecordBaseline(int RecordId);
		char GetRecordGroup(int RecordId);
	}

	public class RedCapService : IRedCapService
	{
		private readonly IConfiguration _configuration;

		public RedCapService(IConfiguration configuration)
		{
			this._configuration = configuration;
		}

		public bool CheckRecordExists(int RecordId)
		{
			throw new NotImplementedException();
		}

		public DateTime GetRecordBaseline(int RecordId)
		{
			throw new NotImplementedException();
		}

		public char GetRecordGroup(int RecordId)
		{
			throw new NotImplementedException();
		}
	}
}
