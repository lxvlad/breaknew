using BreakFree.DAL.Entities;
using System.Collections.Generic;

namespace BreakFree.BLL.Interfaces
{
    public interface IHabitService
    {
        void AddHabit(int userId, string name, DateTime startDate, int goal, string motivation);
        
        List<Habit> GetUserHabits(int userId);
    }
}
