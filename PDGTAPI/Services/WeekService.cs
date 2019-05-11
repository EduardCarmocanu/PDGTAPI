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
		bool HasMoreSessions(User user);
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

			result.Content.Exercises = Exercises(user);
			return result;
		}

		public int RelativeWeek(DateTime date)
		{
			if (date == null)
				throw new ArgumentNullException();

			return (DateTime.Now - date).Days / 7;
		}

		private List<ExerciseDTO> GetExercises(User user)
		{
			if (user == null)
				throw new ArgumentNullException();

			int normalizedCurrentWeek = GetRelativeWeek((DateTime)user.RedCapBaseline) + 1;

			List<ExerciseDTO> exercises = (
				from TimeRangeHasExercise in _context.TimeRangeHasExecise
				join TimeRange in _context.TimeRange on TimeRangeHasExercise.TimeRangeId equals TimeRange.Id
				join Exercise in _context.Exercise on TimeRangeHasExercise.ExerciseId equals Exercise.Id
				where
					TimeRange.StartWeek <= normalizedCurrentWeek &&
					TimeRange.EndWeek >= normalizedCurrentWeek &&
					TimeRange.RandomisationGroupID == user.RandomisationGroupID
					select new ExerciseDTO
					{
						Name = Exercise.ExerciseName,
						Sets = TimeRange.SetsAmount,
					Repetitions = TimeRange.RepsAmount
					}).ToList();

			return exercises;
		}

		public IEnumerable<Session> CompletedSessions(User user)
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

		public bool HasMoreSessions(User user)
		{
			return GetCompletedSessionsInCurrentWeek(user).Count() < _maxWeekSessions;
		}
	}
}
