namespace BreakFree.ConsoleSeed {
    public record User(int Id, string Name, string? Email, string Password, string CreatedAt);
    
    public record Habit(int Id, int UserId, string Name, string StartDate, int GoalDays, string? Motivation, bool IsActive, int TotalDays, int CurrentStreak, decimal TotalSaving);
    public record DailyStatus(int Id, int HabitId, string Date, bool IsClean, string Trigger, string? Note, int CravingLevel);
    public record Achievement(int Id, int UserId, string Title, string AchievedAt);
    public record SOSAction(int Id, string Text, string Category);
    public record UserSOSLog(int Id, int UserId, int ActionId, string Date, bool Worked);
    public record Quote(int Id, string Text);
    public record Saving(int Id, int HabitId, decimal Amount);

    // Test class for Bogus
    public record UserFake
    {
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string CreatedAt { get; init; } = string.Empty;
    }

    public record HabitFake
    {
        public int UserId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string StartDate { get; init; } = string.Empty;
        public int GoalDays { get; init; }
        public string? Motivation { get; init; }
        public int IsActive { get; init; }
        public int TotalDays { get; init; }
        public int CurrentStreak { get; init; }
        public decimal TotalSaving { get; init; }
    }

    public record DailyStatusFake
    {
        public int HabitId { get; init; }
        public string Date { get; init; } = string.Empty;
        public int IsClean { get; init; }
        public string Trigger { get; init; } = string.Empty;
        public string? Note { get; init; }
        public int Craving { get; init; }
    }

    public class AchievementFake
    {
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
        public string AchievedAt { get; set; } = null!;
    }

    public class SOSActionFake
    {
        public string Text { get; set; } = null!;
        public string Category { get; set; } = null!;
    }

    public class UserSOSLogFake
    {
        public int UserId { get; set; }
        public int ActionId { get; set; }
        public string Date { get; set; } = null!;
        public int Worked { get; set; }
    }

    public class QuoteFake
    {
        public string Text { get; set; } = null!;
    }

    public class SavingFake
    {
        public int HabitId { get; set; }
        public decimal Amount { get; set; }
    }
}
