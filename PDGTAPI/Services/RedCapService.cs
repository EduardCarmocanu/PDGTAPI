using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PDGTAPI.DTOs;
using PDGTAPI.Helpers;
using PDGTAPI.Infrastructure;
using PDGTAPI.Models;
using PDGTAPI.Models.Questionnaires;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Exercise = PDGTAPI.Models.Exercise;
using Session = PDGTAPI.Models.Session;

namespace PDGTAPI.Services
{
	public interface IRedCapService
	{
		Task<ServiceResult<UserInfo>> GetRecordInformationAsync(int RecordId);
	}

	public class RedCapService : IRedCapService
	{
		private readonly IConfiguration _configuration;
		private readonly IWeekService _weekService;
		private readonly ApplicationDataContext _context;
		private readonly UserManager<User> _userManager;


		public RedCapService(
			IConfiguration configuration, 
			IWeekService weekService,
			ApplicationDataContext context,
			UserManager<User> userManager
		)
		{
			_configuration = configuration;
			_weekService = weekService;
			_context = context;
			_userManager = userManager;
		}

		public async Task<ServiceResult<UserInfo>> GetRecordInformationAsync (int recordId)
		{
			ServiceResult<UserInfo> result = new ServiceResult<UserInfo>();

			if (recordId < 1)
			{
				result.ErrorMessage = "Record ID < 1 cannot exist";
				return result;
			}

			using (HttpClient client = new HttpClient())
			{
				client.BaseAddress = _configuration.GetValue<Uri>("RedCap:BaseEndpoint");

				using (MultipartFormDataContent content = new MultipartFormDataContent())
				{
					content.Add(new StringContent(_configuration["RedCap:Token"]), "token");
					content.Add(new StringContent("json"), "format");
					content.Add(new StringContent("record"), "content");
					content.Add(new StringContent("flat"), "type");
					content.Add(new StringContent("record_id, date_intervention, randomisation_group"), "fields");
					content.Add(new StringContent("baseline_arm_1"), "events");
					content.Add(new StringContent(recordId.ToString()), "records");

					try
					{
						Task<HttpResponseMessage> message = client.PostAsync(client.BaseAddress, content);

						string responseString = await message.Result.Content.ReadAsStringAsync();
						dynamic[] deserializedResponse = JsonConvert.DeserializeObject<dynamic[]>(responseString);

				if (deserializedResponse.Length < 1)
				{
					result.ErrorMessage = "Record Does not exist";
					return result;
				}

						string responseRecordId = deserializedResponse[0]["record_id"];
						string responseBaselineDate = deserializedResponse[0]["date_intervention"];
						string responseRandomisationGroup = deserializedResponse[0]["randomisation_group"];

						if (string.IsNullOrEmpty(responseRandomisationGroup))
				{
					result.ErrorMessage = "Record does not have a set randomisation group";
					return result;
				}

						if (string.IsNullOrEmpty(responseBaselineDate))
						{
							result.ErrorMessage = "Record does not have a set intervention date";
							return result;
						}

				result.Content = new UserInfo
				{
							RecordId = int.Parse(responseRecordId),
							RandomisationGroup = responseRandomisationGroup,
							BaselineDate = DateTime.Parse(responseBaselineDate).ToUniversalTime()
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
