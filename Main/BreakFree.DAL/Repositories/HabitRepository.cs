using BreakFree.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BreakFree.DAL.Repositories
{
    public class HabitRepository
    {
        public void AddHabit(Habit habit)
        {
            using var context = new BreakFreeContext();
            context.Habits.Add(habit);
            context.SaveChanges();
        }


        public List<Habit> GetHabitsByUser(int userId)
        {
            using var context = new BreakFreeContext();
            return context.Habits.Where(h => h.UserId == userId).ToList();
        }


        public void DeleteHabit(int habitId)
        {
            using var context = new BreakFreeContext();
            var habit = context.Habits.FirstOrDefault(h => h.HabitId == habitId);

            if (habit != null)
            {
                context.Habits.Remove(habit);
                context.SaveChanges();
            }
        }


        public void UpdateHabit(Habit habit)
        {
            using var context = new BreakFreeContext();
            context.Habits.Update(habit);
            context.SaveChanges();
        }
    }
}
