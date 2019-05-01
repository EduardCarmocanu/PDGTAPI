using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PDGTAPI.Helpers;
using PDGTAPI.Models;
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
		ServiceResult<UserInfo> GetRecordInformation(int RecordId);
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

		public ServiceResult<UserInfo> GetRecordInformation (int RecordId)
		{
			ServiceResult<UserInfo> result = new ServiceResult<UserInfo>();

			if (RecordId < 1)
			{
				result.ErrorMessage = "Record ID < 1 cannot exist";
				return result;
			}

			const string Format = "json";
			const string Content = "record";
			const string Type = "flat";

			RestRequest request = new RestRequest(Method.POST);
			request.AddHeader("content-type", "multipart/form-data; boundary=&");

			request.AddHeader("content-type", "multipart/form-data; boundary=&");
			request.AddParameter(
				"multipart/form-data; boundary=&",
				"--&\r\nContent-Disposition: form-data; name=\"token\"\r\n\r\n" +
				_configuration["RedCap:Token"] +
				"\r\n--&\r\nContent-Disposition: form-data; name=\"content\"\r\n\r\n" +
				Content +
				"\r\n--&\r\nContent-Disposition: form-data; name=\"format\"\r\n\r\n" +
				Format +
				"\r\n--&\r\nContent-Disposition: form-data; name=\"records\"\r\n\r\n" +
				RecordId.ToString() +
				"\r\n--&\r\nContent-Disposition: form-data; name=\"type\"\r\n\r\n\\" +
				Type +
				"\r\n--&\r\nContent-Disposition: form-data; name=\"events\"\r\n\r\nbaseline_arm_1\r\n--&\r\nContent-Disposition: form-data; name=\"fields\"\r\n\r\nrecord_id, date_intervention, randomisation_group\r\n--&--",
				ParameterType.RequestBody);

			IRestResponse response = _restClient.Execute(request);

			dynamic[] deserializedResponse = JsonConvert.DeserializeObject<dynamic[]>(response.Content);

			try
			{
				if (deserializedResponse.Length < 1)
				{
					result.ErrorMessage = "Record Does not exist";
					return result;
				}

				string recordId = deserializedResponse[0]["record_id"];
				string baselineDate = deserializedResponse[0]["date_intervention"];
				string randomisationGroupRaw = deserializedResponse[0]["randomisation_group"];

				if (randomisationGroupRaw.Length < 1)
				{
					result.ErrorMessage = "Record does not have a set randomisation group";
					return result;
				}

				char[] randomisationGroup = randomisationGroupRaw.ToCharArray();

				result.Content = new UserInfo
				{
					RecordId = int.Parse(recordId),
					RandomisationGroup = randomisationGroup[0],
					BaselineDate = DateTime.Parse(baselineDate)
				};
				result.Succeded = true;

				return result;
			}
			catch (Exception)
			{
				result.ErrorMessage = "Could not create User Info object";
			}

			return result;
		}
	}
}
