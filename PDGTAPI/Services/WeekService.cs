using PDGTAPI.DTO;
using PDGTAPI.DTOs;
using PDGTAPI.Helpers;
using PDGTAPI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Services
{
	public interface IWeekService
	{
		ServiceResult<WeekStateDTO> GetState(string UserName);
	}

	public class WeekService : IWeekService
	{
		private const int _maxWeekSessions = 3;
		private readonly ApplicationDataContext _context;

		public WeekService(ApplicationDataContext context)
		{
			_context = context;
		}

		public ServiceResult<WeekStateDTO> GetState(string UserName)
		{
			if (UserName == null)
				throw new ArgumentNullException();

			ServiceResult<WeekStateDTO> result = new ServiceResult<WeekStateDTO> {
				Succeded = true,
				Content = new WeekStateDTO()
			};
			User user = _context.Users.FirstOrDefault(x => x.UserName == UserName);

			if (CompletedSessions(user).Count() >= _maxWeekSessions)
			{
				result.Content.SessionsFinished = true;
				return result;
			}

			if (RelativeWeek((DateTime)user.RedCapBaseline) == 0)
			{
				result.Content.Exercises = Exercises(user);
				return result;
			}

			WeeklyQuestionnaire weeklyQuestionnaire = WeeklyQuestionnaire(user);

			if (weeklyQuestionnaire == null)
			{
				CreateQuestionnaire(user);
				result.Content.AvailableWeeklyQuestionnaire = true;
				return result;
			}

			if (!weeklyQuestionnaire.Completed)
			{
				result.Content.AvailableWeeklyQuestionnaire = true;
				return result;
			}

			result.Content.Exercises = Exercises(user);
			return result;
		}

		private int RelativeWeek(DateTime date)
		{
			if (date == null)
				throw new ArgumentNullException();

			return (DateTime.Now - date).Days / 7;
		}

		private WeeklyQuestionnaire WeeklyQuestionnaire(User user)
		{
			if (user == null)
				throw new ArgumentNullException();
			/**
			 * If the creation date is in the same week as the current date
			 * then the weekly questionnaire is refering to the previous week.
			 * As a rule of thumb: The weekly questionnaire alway looks at the previous week
			 * 
			 * This is because the entry creation is event driven. They are created
			 * when the client requests week state and the system sees that there has not been a 
			 * questionnaire created for the previous week
			 **/
			return _context.WeeklyQuestionnaire.FirstOrDefault(
				w => w.UserId == user.Id && RelativeWeek(w.CreationTime) == 0
			);
		}

		private List<ExerciseDTO> Exercises(User user)
		{
			if (user == null)
				throw new ArgumentNullException();

			/**
			 * It's a big query...but it works ¯\_(ツ)_/¯
			 * **/
			List<ExerciseDTO> exercises = (
				from UserHasExerciseWeightInTimeRange in _context.UserHasExerciseWeightInTimeRange
					join Exercise in _context.Exercise on UserHasExerciseWeightInTimeRange.ExerciseId equals Exercise.Id
					join User in _context.Users on UserHasExerciseWeightInTimeRange.UserId equals User.Id
					join Guide in _context.Guide on Exercise.GuideId equals Guide.Id
					join TimeRange in _context.TimeRange on UserHasExerciseWeightInTimeRange.TimeRangeId equals TimeRange.Id
					select new ExerciseDTO
					{
						Name = Exercise.ExerciseName,
						Weight = UserHasExerciseWeightInTimeRange.UserExerciseWeight,
						Sets = TimeRange.SetsAmount,
						Repetitions = TimeRange.RepsAmount,
						Guide = new GuideDTO
						{
							Description = Guide.GuideDescription,
							Image = Guide.GuideImage
						}
					}).ToList();

			return exercises;
		}

		private IEnumerable<Session> CompletedSessions(User user)
		{
			if (user == null)
				throw new ArgumentNullException();

			DateTime userBaseline = (DateTime)user.RedCapBaseline;
			int currentRelativeWeek = RelativeWeek(userBaseline);

			DateTime startTime = userBaseline.AddDays(7 * currentRelativeWeek);
			DateTime endTime = startTime.AddDays(7);

			return _context.Session.Where(
				s => s.UserId == user.Id 
					&& s.CompletionTime >= startTime 
					&& s.CompletionTime <= endTime
			);
		}

		private void CreateQuestionnaire(User user)
		{
			var result = _context.WeeklyQuestionnaire.Add(new WeeklyQuestionnaire
			{
				UserId = user.Id,
				Completed = false,
				CreationTime = DateTime.Now
			});
		}
	}
}
