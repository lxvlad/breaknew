using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BreakFree.DAL.Entities
{
    [Table("Habits")]
    public class Habit
    {
        [Column("habit_id")]
        public int HabitId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("habit_name")]
        public string HabitName { get; set; } = string.Empty;

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("daily_goal")]
        public int DailyGoal { get; set; }

        [Column("motivation")]
        public string Motivation { get; set; } = string.Empty;

        [Column("is_active")]
        public bool IsActive { get; set; }
    }
}
