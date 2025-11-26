using System;
using System.Collections.Generic;
using BreakFree.DAL.Entities;
using System.Collections.Generic;
using System.Linq;


namespace BreakFree.DAL.Repositories
{
    public class DailyStatusRepository
    {
        public void AddDailyStatus(DailyStatus status)
        {
            using var context = new BreakFreeContext();
            context.DailyStatuses.Add(status);
            context.SaveChanges();
        }


        public List<DailyStatus> GetStatusesByHabit(int habitId)
        {
            using var context = new BreakFreeContext();
            return context.DailyStatuses.Where(s => s.HabitId == habitId).ToList();
        }


        public void UpdateDailyStatus(DailyStatus status)
        {
            using var context = new BreakFreeContext();
            context.DailyStatuses.Update(status);
            context.SaveChanges();
        }


        public void DeleteDailyStatus(int statusId)
        {
            using var context = new BreakFreeContext();
            var status = context.DailyStatuses.FirstOrDefault(s => s.StatusId == statusId);

            if (status != null)
            {
                context.DailyStatuses.Remove(status);
                context.SaveChanges();
            }
        }


        public List<DailyStatus> GetStatusesByUser(int userId)
        {
            using var context = new BreakFreeContext();
            return context.DailyStatuses
                        .Where(s => context.Habits.Any(h => h.HabitId == s.HabitId && h.UserId == userId))
                        .ToList();
        }
    }
}
