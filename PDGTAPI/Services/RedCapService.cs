using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PDGTAPI.Services
{
	public interface IRedCapService
	{
		bool RecordExists(int RecordId);
		DateTime GetRecordBaseline(int RecordId);
		char? GetRecordGroup(int RecordId);
	}

	public class RedCapService : IRedCapService
	{
		private readonly IConfiguration _configuration;
		private readonly IRestClient _restClient; 

		public RedCapService(IConfiguration configuration, IRestClient restClient)
		{
			_configuration = configuration;
			_restClient = restClient;
			restClient.BaseUrl = _configuration.GetValue<Uri>("RedCap:BaseEndpoint");
		}

		public bool RecordExists(int RecordId)
		{
			if (RecordId == 0)
				throw new ArgumentNullException("RecordId must be > 0");

			RestRequest request = new RestRequest(Method.POST);

			request.AddHeader("content-type", "multipart/form-data; boundary=&");

			/**
			 * RestSharp code snipped generated from PostMan.
			 * Includes as part of the form-data parameters the search RecordId which is used to check if there is an existing record for the record
			 */
			request.AddParameter(
				"multipart/form-data; boundary=&",
				"--&\r\nContent-Disposition: form-data; name=\"token\"\r\n\r\n" +
				_configuration["RedCap:Token"] +
				"\r\n--&\r\nContent-Disposition: form-data; name=\"content\"\r\n\r\nrecord\r\n--&\r\nContent-Disposition: form-data; name=\"format\"\r\n\r\njson\r\n--&\r\nContent-Disposition: form-data; name=\"records\"\r\n\r\n" +
				RecordId.ToString() +
				"\r\n--&\r\nContent-Disposition: form-data; name=\"type\"\r\n\r\nflat\r\n--&\r\nContent-Disposition: form-data; name=\"events\"\r\n\r\nbaseline_arm_1\r\n--&\r\nContent-Disposition: form-data; name=\"fields\"\r\n\r\nrecord_id\r\n--&--",
				ParameterType.RequestBody
			);

			IRestResponse response = _restClient.Execute(request);
			return JsonConvert.DeserializeObject<object[]>(response.Content).Length == 1;
		}	

		public DateTime GetRecordBaseline(int RecordId)
		{
			if (RecordId == 0)
				throw new ArgumentNullException("RecordId must be > 0");

			throw new NotImplementedException();
		}

		public char? GetRecordGroup(int RecordId)
		{
			if (RecordId == 0)
				throw new ArgumentNullException("RecordId must be > 0");

			RestRequest request = new RestRequest(Method.POST);

			request.AddHeader("content-type", "multipart/form-data; boundary=&");
			request.AddParameter(
				"multipart/form-data; boundary=&",
				"--&\r\nContent-Disposition: form-data; name=\"token\"\r\n\r\n" +
				_configuration["RedCap:Token"] +
				"\r\n--&\r\nContent-Disposition: form-data; name=\"content\"\r\n\r\nrecord\r\n--&\r\nContent-Disposition: form-data; name=\"format\"\r\n\r\njson\r\n--&\r\nContent-Disposition: form-data; name=\"records\"\r\n\r\n" +
				RecordId.ToString() +
				"\r\n--&\r\nContent-Disposition: form-data; name=\"type\"\r\n\r\nflat\r\n--&\r\nContent-Disposition: form-data; name=\"events\"\r\n\r\nbaseline_arm_1\r\n--&\r\nContent-Disposition: form-data; name=\"fields\"\r\n\r\nrandomisation_group\r\n--&--",
				ParameterType.RequestBody);

			IRestResponse response = _restClient.Execute(request);

			char group;
			try
			{
				group = (char)JsonConvert.DeserializeObject<object[]>(response.Content)[0];
			}
			catch (Exception)
			{
				return null;
			}

			if (group == 'A' || group == 'B') return group;

			return null;
		}
	}
}
