using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Bogus;
using System.Linq;

namespace BreakFree.ConsoleSeed
{
    public static class Seed
    {
        private const int MinCount = 30;
        private const int MaxCount = 50;
        private static Random Rng = new Random();

        public static void SeedWithBogus(string connectionString)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            Console.WriteLine("[INFO] Seeding database with Bogus-generated data...");

            // --- USERS ---
            var userFaker = new Faker<UserFake>("uk")
                .RuleFor(u => u.Name, f => f.Person.FullName)
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => f.Internet.Password())
                .RuleFor(u => u.CreatedAt, f => f.Date.Past(2).ToString("yyyy-MM-dd"));

            var users = userFaker.Generate(Rng.Next(MinCount, MaxCount));
            foreach (var u in users)
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO Users (user_name,email,password,created_at) VALUES (@name,@email,@password,@created)";
                cmd.Parameters.AddWithValue("@name", u.Name);
                cmd.Parameters.AddWithValue("@email", u.Email);
                cmd.Parameters.AddWithValue("@password", u.Password);
                cmd.Parameters.AddWithValue("@created", u.CreatedAt);
                cmd.ExecuteNonQuery();
            }

            var userIds = GetIds(connection, "Users", "user_id");

            // --- HABITS ---
            var habitFaker = new Faker<HabitFake>("uk")
                .RuleFor(h => h.UserId, f => f.PickRandom(userIds))
                .RuleFor(h => h.Name, f => f.Lorem.Word())
                .RuleFor(h => h.StartDate, f => f.Date.Past(1).ToString("yyyy-MM-dd"))
                .RuleFor(h => h.GoalDays, f => f.Random.Int(10, 90))
                .RuleFor(h => h.Motivation, f => f.Random.Bool() ? f.Lorem.Sentence() : null)
                .RuleFor(h => h.IsActive, f => f.Random.Bool() ? 1 : 0)
                .RuleFor(h => h.TotalDays, f => f.Random.Int(1, 100))
                .RuleFor(h => h.CurrentStreak, f => f.Random.Int(0, 20))
                .RuleFor(h => h.TotalSaving, f => f.Random.Decimal(0, 5000));

            var habits = habitFaker.Generate(Rng.Next(MinCount, MaxCount));
            foreach (var h in habits)
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO Habits 
                    (user_id, habit_name, start_date, goal_days, motivation, is_active, total_days, current_streak, total_saving)
                    VALUES (@uid,@name,@start,@goal,@mot,@active,@total,@streak,@saving)";
                cmd.Parameters.AddWithValue("@uid", h.UserId);
                cmd.Parameters.AddWithValue("@name", h.Name);
                cmd.Parameters.AddWithValue("@start", h.StartDate);
                cmd.Parameters.AddWithValue("@goal", h.GoalDays);
                cmd.Parameters.AddWithValue("@mot", (object?)h.Motivation ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@active", h.IsActive);
                cmd.Parameters.AddWithValue("@total", h.TotalDays);
                cmd.Parameters.AddWithValue("@streak", h.CurrentStreak);
                cmd.Parameters.AddWithValue("@saving", h.TotalSaving);
                cmd.ExecuteNonQuery();
            }

            var habitIds = GetIds(connection, "Habits", "habit_id");

            // --- DAILY STATUSES ---
            var statusDates = new Dictionary<int, HashSet<string>>();
            var statusFaker = new Faker<DailyStatusFake>("uk")
                .RuleFor(s => s.HabitId, f =>
                {
                    int habitId = f.PickRandom(habitIds);
                    if (!statusDates.ContainsKey(habitId))
                        statusDates[habitId] = new HashSet<string>();
                    return habitId;
                })
                .RuleFor(s => s.Date, (f, s) =>
                {
                    string date;
                    do
                    {
                        date = f.Date.Recent(60).ToString("yyyy-MM-dd");
                    } while (statusDates[s.HabitId].Contains(date));
                    statusDates[s.HabitId].Add(date);
                    return date;
                })
                .RuleFor(s => s.IsClean, f => f.Random.Bool() ? 1 : 0)
                .RuleFor(s => s.Trigger, f => f.Lorem.Word())
                .RuleFor(s => s.Note, f => f.Random.Bool() ? f.Lorem.Sentence() : null)
                .RuleFor(s => s.Craving, f => f.Random.Int(0, 10));

            var statuses = statusFaker.Generate(Rng.Next(MinCount, MaxCount));
            foreach (var s in statuses)
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO DailyStatuses 
                    (habit_id, date, is_clean, trigger, note, craving_level)
                    VALUES (@hid,@date,@clean,@trig,@note,@craving)";
                cmd.Parameters.AddWithValue("@hid", s.HabitId);
                cmd.Parameters.AddWithValue("@date", s.Date);
                cmd.Parameters.AddWithValue("@clean", s.IsClean);
                cmd.Parameters.AddWithValue("@trig", s.Trigger);
                cmd.Parameters.AddWithValue("@note", (object?)s.Note ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@craving", s.Craving);
                cmd.ExecuteNonQuery();
            }

            // --- ACHIEVEMENTS ---
            var achievementFaker = new Faker<AchievementFake>("uk")
                .RuleFor(a => a.UserId, f => f.PickRandom(userIds))
                .RuleFor(a => a.Title, f => f.Lorem.Sentence())
                .RuleFor(a => a.AchievedAt, f => f.Date.Past(2).ToString("yyyy-MM-dd"));

            var achievements = achievementFaker.Generate(Rng.Next(MinCount, MaxCount));
            foreach (var a in achievements)
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO Achievements (user_id, title, achieved_at) VALUES (@uid,@title,@achieved)";
                cmd.Parameters.AddWithValue("@uid", a.UserId);
                cmd.Parameters.AddWithValue("@title", a.Title);
                cmd.Parameters.AddWithValue("@achieved", a.AchievedAt);
                cmd.ExecuteNonQuery();
            }

            // --- SOS ACTIONS ---
            var sosFaker = new Faker<SOSActionFake>("uk")
                .RuleFor(s => s.Text, f => f.Lorem.Sentence())
                .RuleFor(s => s.Category, f => f.PickRandom(new[] { "Health", "Mind", "Social", "Other" }));

            var sosActions = sosFaker.Generate(Rng.Next(MinCount, MaxCount));
            foreach (var s in sosActions)
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO SOSActions (text, category) VALUES (@text,@category)";
                cmd.Parameters.AddWithValue("@text", s.Text);
                cmd.Parameters.AddWithValue("@category", s.Category);
                cmd.ExecuteNonQuery();
            }

            var sosIds = GetIds(connection, "SOSActions", "action_id");

            // --- USER SOS LOGS ---
            var userSOSFaker = new Faker<UserSOSLogFake>("uk")
                .RuleFor(u => u.UserId, f => f.PickRandom(userIds))
                .RuleFor(u => u.ActionId, f => f.PickRandom(sosIds))
                .RuleFor(u => u.Date, f => f.Date.Recent(60).ToString("yyyy-MM-dd"))
                .RuleFor(u => u.Worked, f => f.Random.Bool() ? 1 : 0);

            var userSOSLogs = userSOSFaker.Generate(Rng.Next(MinCount, MaxCount));
            foreach (var log in userSOSLogs)
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO UserSOSLogs (user_id, action_id, date, worked) VALUES (@uid,@aid,@date,@worked)";
                cmd.Parameters.AddWithValue("@uid", log.UserId);
                cmd.Parameters.AddWithValue("@aid", log.ActionId);
                cmd.Parameters.AddWithValue("@date", log.Date);
                cmd.Parameters.AddWithValue("@worked", log.Worked);
                cmd.ExecuteNonQuery();
            }

            // --- QUOTES ---
            var quoteFaker = new Faker<QuoteFake>("uk")
                .RuleFor(q => q.Text, f => f.Lorem.Sentence());

            var quotes = quoteFaker.Generate(Rng.Next(MinCount, MaxCount));
            foreach (var q in quotes)
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO Quotes (text) VALUES (@text)";
                cmd.Parameters.AddWithValue("@text", q.Text);
                cmd.ExecuteNonQuery();
            }

            // --- SAVINGS ---
            var savingFaker = new Faker<SavingFake>("uk")
                .RuleFor(s => s.HabitId, f => f.PickRandom(habitIds))
                .RuleFor(s => s.Amount, f => f.Random.Decimal(10, 500));

            var savings = savingFaker.Generate(Rng.Next(MinCount, MaxCount));
            foreach (var s in savings)
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO Savings (habit_id, amount) VALUES (@hid,@amount)";
                cmd.Parameters.AddWithValue("@hid", s.HabitId);
                cmd.Parameters.AddWithValue("@amount", s.Amount);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("[INFO] Seeding complete!");
        }

        private static int[] GetIds(SqliteConnection connection, string table, string idColumn)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT {idColumn} FROM {table}";
            using var reader = cmd.ExecuteReader();
            var ids = new List<int>();
            while (reader.Read())
                ids.Add(reader.GetInt32(0));
            return ids.ToArray();
        }
    }
}
