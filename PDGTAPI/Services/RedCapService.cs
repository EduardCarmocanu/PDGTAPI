using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PDGTAPI.DTOs;
using PDGTAPI.Helpers;
using PDGTAPI.Infrastructure;
using PDGTAPI.Models;
using PDGTAPI.Models.Questionnaires;
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
		Task<ServiceResult<string>> PostSessionQuestionnaireAsync(Session sessionQuestionnaire, string userName);
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

		public async Task<ServiceResult<string>> PostSessionQuestionnaireAsync(Session sessionQuestionnaire, string userName)
		{
			if (sessionQuestionnaire == null || userName == null)
				throw new ArgumentNullException();

			ServiceResult<string> result = new ServiceResult<string>();
			User user = _context.Users.SingleOrDefault(u => u.UserName == userName);
			IEnumerable<Infrastructure.Session> sessions = _weekService.GetCompletedSessionsInCurrentWeek(user);

			if (!_weekService.PatientCanTrain(sessions)) 
			{
				result.ErrorMessage = "No more sessions available today";
				return result;
			}

			int currentRelativeWeek = _weekService.GetRelativeWeek((DateTime)user.RedCapBaseline);

			TimeRange timeRange = (
				from TimeRange in _context.TimeRange
				join RandomisationGroup in _context.RandomisationGroup
					on TimeRange.RandomisationGroupID equals user.RandomisationGroupID
				where
					TimeRange.RandomisationGroupID == user.RandomisationGroupID &&
					TimeRange.StartWeek <= (currentRelativeWeek + 1) &&
					TimeRange.EndWeek >= (currentRelativeWeek + 1)
				select new TimeRange
				{
					StartWeek = TimeRange.StartWeek,
					EndWeek = TimeRange.EndWeek,
					RedCapIdentifier = TimeRange.RedCapIdentifier,
					RandomisationGroup = RandomisationGroup.GroupName
				}).ToList().First();

			string identifier = "";

			if (!string.IsNullOrEmpty(timeRange.RedCapIdentifier))
			{
				identifier = "_" + timeRange.RedCapIdentifier;
			}

			ExtractPainkillers(
				sessionQuestionnaire.PreQuestionnaire.Painkillers,
				out string painkillersListBefore,
				out int? painkillersQuantityBefore
			);

			ExtractPainkillers(
				sessionQuestionnaire.PostQuestionnaire.Painkillers,
				out string painkillersListAfter,
				out int? painkillersQuantityAfter
			);

			Dictionary<string, object> postObject = new Dictionary<string, object>
			{
				{ "record_id", user.RedCapRecordId },
				{ "redcap_event_name", "log_" + (currentRelativeWeek * 3 + sessions.Count() + 1).ToString() + "_arm_1" },
				{ "log_training" + identifier, 1 },
				{ "log_supervised" + identifier, 0 },
				{ "log_date" + identifier, DateTime.UtcNow.ToString("yyyy-MM-dd") },
				{ "log_pain_before" + identifier, sessionQuestionnaire.PreQuestionnaire.Pain },
				{ "log_painkiller_before" + identifier, sessionQuestionnaire.PreQuestionnaire.TakenPainkillers ? 1 : 0 },
				{ "log_painkiller_type_before" + identifier, painkillersListBefore},
				{ "log_painkiller_amount_before" + identifier, painkillersQuantityBefore },
				{ "log_pain_after" + identifier, sessionQuestionnaire.PostQuestionnaire.Pain },
				{ "log_painkiller_after" + identifier, sessionQuestionnaire.PostQuestionnaire.TakenPainkillers ? 1 : 0 },
				{ "log_painkiller_type_after" + identifier, painkillersListAfter },
				{ "log_painkiller_amount_after" + identifier, painkillersQuantityAfter },
				{ "log_sideeffect" + identifier, sessionQuestionnaire.PostQuestionnaire.TrainingSideEffects ? 1 : 0 },
				{ "log_sideeffect_des" + identifier, sessionQuestionnaire.PostQuestionnaire.TrainingSideEffectsDescription },
				{ "log_comment" + identifier, sessionQuestionnaire.PostQuestionnaire.Comments },
			};

			string key;

			switch (timeRange.RandomisationGroup)
			{
				case Groups.Intervention:
					postObject.Add("log_fatigue" + identifier, sessionQuestionnaire.PostQuestionnaire.Tired ? 1 : 0);
					postObject.Add("intervention_training_log_week_" + timeRange.StartWeek + timeRange.EndWeek + "_complete", 2);

					key = "weight";
					for (int i = 0, n = sessionQuestionnaire.Exercises.Count(); i < n; i++)
					{
						for (int j = 0, k = sessionQuestionnaire.Exercises[i].Sets.Count; j < k; j++)
						{
							postObject.Add(
								"log_" + (i + 1).ToString() + "_" + (j + 1).ToString() + "_reps" + identifier, 
								sessionQuestionnaire.Exercises[i].Sets[j].Repetitions
							);
							postObject.Add(
								"log_" + (i + 1).ToString() + "_" + (j + 1).ToString() + "_" + key + identifier, 
								sessionQuestionnaire.Exercises[i].Sets[j].Weight
							);
						}
					}
					break;
				case Groups.Control:
					postObject.Add("kontrol_training_log_week_" + timeRange.StartWeek + timeRange.EndWeek + "_complete", 2);

					key = "fatigue";
					for (int i = 0, n = sessionQuestionnaire.Exercises.Count(); i <= n; i++)
					{
						for (int j = 0, k = sessionQuestionnaire.Exercises[i].Sets.Count; j <= k; j++)
						{
							postObject.Add(
								"log_" + (i + 1).ToString() + "_" + (j + 1).ToString() + "_reps" + identifier, 
								sessionQuestionnaire.Exercises[i].Sets[j].Repetitions
							);
							postObject.Add(
								"log_" + (i + 1).ToString() + "_" + (j + 1).ToString() + "_" + key + identifier, 
								sessionQuestionnaire.Exercises[i].Sets[j].Tired
							);
						}
					}
					break;
				default:
					throw new ArgumentException();
			}

			using (HttpClient client = new HttpClient())
			{
				client.BaseAddress = _configuration.GetValue<Uri>("RedCap:BaseEndpoint");

				using (MultipartFormDataContent content = new MultipartFormDataContent())
				{
					content.Add(new StringContent(_configuration["RedCap:Token"]), "token");
					content.Add(new StringContent("json"), "format");
					content.Add(new StringContent("record"), "content");
					content.Add(new StringContent(JsonConvert.SerializeObject(new Dictionary<string, object>[] { postObject })), "data");

					Task<HttpResponseMessage> message = client.PostAsync(client.BaseAddress, content);

					if (message.Result.IsSuccessStatusCode)
					{
						result.Content = await message.Result.Content.ReadAsStringAsync();
						result.Succeded = true;

						_context.Session.Add(new Infrastructure.Session()
						{
							CompletionTime = DateTime.Now,
							UserId = user.Id
						});
						await _context.SaveChangesAsync();

						return result;
					}

					result.ErrorMessage = "Could not create resource";
					return result;
				} 
			}
		}

		private void ExtractPainkillers(List<Painkiller> painkillers, out string painkillersList, out int? painkillersQuantity)
		{
			painkillersList = null;
			painkillersQuantity = 0;

			if (painkillers.Count() > 0)
			{
				for (int i = 0, n = painkillers.Count(); i < n - 1; i++)
				{
					painkillersList += painkillers[i].Type + ", ";
					painkillersQuantity += painkillers[i].Amount;
				}

				// Adds the last type without a comma
				painkillersList += painkillers.Last().Type;
				painkillersQuantity += painkillers.Last().Amount;
			}

			// Sets value to a number string if it's greater than 0
			if (painkillersQuantity == 0)
			{
				painkillersQuantity = null;
			}
		}
	}
}
