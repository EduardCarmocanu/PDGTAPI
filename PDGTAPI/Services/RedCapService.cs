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
		ServiceResult<bool> RecordExists(int RecordId);
		ServiceResult<DateTime> GetRecordBaseline(int RecordId);
		ServiceResult<char> GetRecordGroup(int RecordId);
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

		public ServiceResult<bool> RecordExists(int RecordId)
		{
			if (RecordId == 0)
			{
				throw new ArgumentException("RecordId must be > 0");
			}

			ServiceResult<bool> serviceResult = new ServiceResult<bool>();
			RestRequest request = new RestRequest(Method.POST);

			request.AddHeader("content-type", "multipart/form-data; boundary=&");
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

			dynamic deserializedResponseContent;
			try
			{
				deserializedResponseContent = JsonConvert.DeserializeObject<dynamic>(response.Content);
				if (deserializedResponseContent.First != null)
				{
					deserializedResponseContent = deserializedResponseContent.First["record_id"].Value;
				}
				else
				{
					serviceResult.ErrorMessage = "Empty response";
					return serviceResult;
				}
			}
			catch (Exception)
			{
				serviceResult.ErrorMessage = "Error deserializing request response";
				return serviceResult;
			}
			 

			if (int.TryParse(deserializedResponseContent, out int result)) // int result variable is declared inline
			{
				serviceResult.Content = result == RecordId;
				serviceResult.Succeded = true;

				return serviceResult;
			}

			serviceResult.ErrorMessage = "Error casting reponse result to int";
			return serviceResult;
		}

		public ServiceResult<DateTime> GetRecordBaseline(int RecordId)
		{
			if (RecordId == 0)
				throw new ArgumentException("RecordId must be > 0");

			throw new NotImplementedException();
		}

		public ServiceResult<char> GetRecordGroup(int RecordId)
		{
			if (RecordId == 0)
				throw new ArgumentException("RecordId must be > 0");

			ServiceResult<char> serviceResult = new ServiceResult<char>();
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

			string deserializedResponseContent;
			try
			{
				deserializedResponseContent = JsonConvert.DeserializeObject<dynamic>(response.Content)[0]["randomisation_group"].Value;
			}
			catch (Exception)
			{
				serviceResult.ErrorMessage = "Failed to deserialize request response";
				return serviceResult;
			}

			if (char.TryParse(deserializedResponseContent, out char result)) // char result variable is declared inline
			{
				if (result == Groups.Intervention || result == Groups.Control)
				{
					serviceResult.Content = result; 
					serviceResult.Succeded = true;

					return serviceResult;
				}

				serviceResult.ErrorMessage = "Patient group is not assigned or not part of the set {A, B}";

				return serviceResult;
			}

			serviceResult.ErrorMessage = "Failed to parse request response";
			return serviceResult;
		}
	}
}
