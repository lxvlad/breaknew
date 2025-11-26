using BreakFree.BLL.Interfaces;
using BreakFree.DAL.Entities;
using BreakFree.DAL.Repositories;
using System;
using System.Collections.Generic;


namespace BreakFree.BLL.Services
{
    public class HabitService : IHabitService
    {

        private readonly HabitRepository _habitRepository;


        public HabitService()
        {
            _habitRepository = new HabitRepository();
        }


        public void AddHabit(int userId, string name, DateTime startDate, int goal, string motivation)
        {

            var habit = new Habit
            {
                UserId = userId,
                HabitName = name,
                StartDate = startDate,
                DailyGoal = goal,
                Motivation = motivation,
                IsActive = true
            };

            _habitRepository.AddHabit(habit);
        }


        public List<Habit> GetUserHabits(int userId)
        {
            return _habitRepository.GetHabitsByUser(userId);
        }
    }
}
